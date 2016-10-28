/*----------------------------------------------------------------
    Copyright (C) 2016 Senparc
    
    文件名：OAuth2Controller.cs
    文件功能描述：提供OAuth2.0授权测试（关注微信公众号：盛派网络小助手，点击菜单【功能体验】 【OAuth2.0授权测试】即可体验）
    
    
    创建标识：Senparc - 20150312
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using System.Threading.Tasks;

namespace Senparc.Weixin.MP.Sample.Controllers
{
    public class OAuth2Controller : Controller
    {
        //下面换成账号对应的信息，也可以放入web.config等地方方便配置和更换
        private string appId = ConfigurationManager.AppSettings["TenPayV3_AppId"];
        private string secret = ConfigurationManager.AppSettings["TenPayV3_AppSecret"];


        private static Dictionary<Guid, OAuthUserInfo> dicSessionList = new Dictionary<Guid, OAuthUserInfo>();
        public ActionResult Login()
        {
            var token = Guid.NewGuid();
            if (Session["OAuthAccessToken"] == null)
            {
                Session["OAuthAccessToken"] = token;
            }
            else
            {
                token = (Guid) Session["OAuthAccessToken"];
                var defaultOAuthUserInfo = default(OAuthUserInfo);
                var autoUserInfo = dicSessionList.TryGetValue(token, out defaultOAuthUserInfo);
                
            }
            
            return RedirectToAction("Index", new {url = "http://www.soyotu.com/OAuth2/UserInfoCallback", tokenHc = token.GetHashCode()});
        }

       
        public ActionResult Index(string url, string tokenHc)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "http://www.soyotu.com/OAuth2/UserInfoCallback";
            }
            Guid token = (Guid)Session["OAuthAccessToken"];
            //var token = CommonApi.GetToken(appId, secret);// OAuthApi.GetAccessToken(appId, secret, code);
            //var task = UserApi.Get(token.access_token, string.Empty);
            //var listOpenids = task.data.openid;
            var listUser = new List<UserInfoJson>();
            //foreach (var openid in listOpenids)
            //{
            //    var user = UserApi.Info(token.access_token, openid);
            //    listUser.Add(user);
            //}

            //此页面引导用户点击授权
            ViewData["UrlUserInfo"] = OAuthApi.GetAuthorizeUrl(appId, url, tokenHc, OAuthScope.snsapi_userinfo);
            var urlBase = OAuthApi.GetAuthorizeUrl(appId, url, tokenHc, OAuthScope.snsapi_base);
            ViewData["UrlBase"] = urlBase;
            //return View(listUser);
            return Redirect(urlBase);
        }


        static Dictionary<string, OAuthAccessTokenResult> OAuthCodeCollection = new Dictionary<string, OAuthAccessTokenResult>();
        static object OAuthCodeCollectionLock = new object();

        /// <summary>
        /// OAuthScope.snsapi_userinfo方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }
            Guid token = (Guid)Session["OAuthAccessToken"];
            if (token.GetHashCode().ToString() != state)
            {
                return Content("验证失败！请从正规途径进入！");
            }

            //if (state != "JeffreySu")
            //{
            //    //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
            //    //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
            //    return Content("验证失败！请从正规途径进入！");
            //}

            string openId;
            OAuthAccessTokenResult result = null;

            #region 解決 Auth 出現 40028(Invalid code)錯誤
            try
            {
                //通过，用code换取access_token

                var isSecondRequest = false;
                lock (OAuthCodeCollectionLock)
                {
                    isSecondRequest = OAuthCodeCollection.ContainsKey(code);
                }

                if (!isSecondRequest)
                {
                    //第一次请求
                    //LogUtility.Weixin.DebugFormat("第一次微信OAuth到达，code：{0}", code);
                    lock (OAuthCodeCollectionLock)
                    {
                        OAuthCodeCollection[code] = null;
                    }
                }
                else
                {
                    //第二次请求
                    //LogUtility.Weixin.DebugFormat("第二次微信OAuth到达，code：{0}", code);

                    lock (OAuthCodeCollectionLock)
                    {
                        result = OAuthCodeCollection[code];
                    }
                }

                try
                {
                    try
                    {
                        result = result ?? OAuthApi.GetAccessToken(appId, secret, code);
                    }
                    catch (Exception ex)
                    {
                        return Content("OAuth AccessToken错误：" + ex.Message);
                    }

                    if (result != null)
                    {
                        lock (OAuthCodeCollectionLock)
                        {
                            OAuthCodeCollection[code] = result;
                        }
                    }
                }
                catch (ErrorJsonResultException ex)
                {
                    if (ex.JsonResult.errcode == ReturnCode.不合法的oauth_code)
                    {
                        //code已经被使用过
                        lock (OAuthCodeCollectionLock)
                        {
                            result = OAuthCodeCollection[code];
                        }
                    }
                }

                openId = result != null ? result.openid : null;
            }
            catch (Exception ex)
            {
                return Content("授权过程发生错误：" + ex.Message);
            }
            #endregion

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                var userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                var res = new JsonResult();
                res.Data = new {OpenId = userInfo.openid, NickName = userInfo.nickname};
                res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。  
                dicSessionList.Add(token, userInfo);
                return View(userInfo);
                //return res;
            }
            catch (ErrorJsonResultException ex)
            {
                return Content("OAuthApi.GetUserInfoAsync(result.access_token, result.openid):::  " + ex.Message);
            }
        }
 
        /// <summary>
        /// OAuthScope.snsapi_base方式回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult BaseCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != "JeffreySu")
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(appId, secret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
            OAuthUserInfo userInfo = null;
            try
            {
                //已关注，可以得到详细信息
                userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                ViewData["ByBase"] = true;
                return View("UserInfoCallback", userInfo);
            }
            catch (ErrorJsonResultException ex)
            {
                //未关注，只能授权，无法得到详细信息
                //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                return Content("用户已授权，授权Token：" + result);
            }
        }
    }
}