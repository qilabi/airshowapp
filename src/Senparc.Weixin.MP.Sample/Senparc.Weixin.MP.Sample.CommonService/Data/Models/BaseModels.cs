using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Senparc.Weixin.MP.Sample.CommonService.Data.Models
{
   public abstract class BaseModels<T,TKey> : Entity<TKey>,IDataErrorInfo, IChangeTracking, IDisposable
        where T : BaseModels<T, TKey>, new()
    {

        #region Constants and Fields

        /// <summary>
        /// The broken rules.
        /// </summary>
        public readonly Dictionary<String, String> _brokenRules = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);

       
        protected BaseModels()
        {
            //this.New = true;
            this.IsChanged = true;
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the class is Saved
        /// </summary>
        public static event EventHandler<SavedEventArgs> Saved;

        /// <summary>
        ///     Occurs when the class is Saved
        /// </summary>
        public static event EventHandler<SavedEventArgs> Saving;

        #endregion
        [Ignore]
        #region Properties
        public string Error
        {
            get
            {
                return this.ValidationMessage;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether if this object is marked for deletion.
        /// </summary>
        [Ignore]
        public bool Deleted { get;  set; }

        [Ignore]
        public bool IsChanged { get; private set; }

        /// <summary>
        ///     Gets a value indicating whether if this is a new object, False if it is a pre-existing object.
        /// </summary>
        /// 
        [Ignore]
        public bool New { get;  set; }

        /// <summary>
        ///     Gets a value indicating whether the object is valid or not.
        /// </summary>
        [Ignore]
        public bool Valid
        {
            get
            {
                this.ValidationRules();
                return this._brokenRules.Count == 0;
            }
        }

        /// ///
        /// <summary>
        ///     Gets if the object has broken business rules, use this property to get access
        ///     to the different validation messages.
        /// </summary>
        [Ignore]
        public virtual string ValidationMessage
        {
            get
            {
                if (!this.Valid)
                {
                    var sb = new StringBuilder();
                    foreach (string messages in this._brokenRules.Values)
                    {
                        sb.AppendLine(messages);
                    }

                    return sb.ToString();
                }

                return string.Empty;
            }
        }
       
        [Ignore]
        protected bool Disposed { get; private set; }

        #endregion

        #region Indexers

        /// <summary>
        ///     Gets the <see cref = "System.String" /> with the specified column name.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        public string this[string columnName]
        {
            get
            {
                return this._brokenRules.ContainsKey(columnName) ? this._brokenRules[columnName] : string.Empty;
            }
        }

        #endregion
        
        /// <summary>
        /// Marks the object for deletion. It will then be 
        ///     deleted when the object's Save() method is called.
        /// </summary>
        public virtual void Delete()
        {
            this.Deleted = true;
            this.IsChanged = true;
        }


        /// <summary>
        /// Comapares this object with another
        /// </summary>
        /// <param name="obj">
        /// The object to compare
        /// </param>
        /// <returns>
        /// True if the two objects as equal
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj != null && (obj.GetType() == this.GetType() && obj.GetHashCode() == this.GetHashCode());
        }

        /// <summary>
        /// A uniquely key to identify this particullar instance of the class
        /// </summary>
        /// <returns>
        /// A unique integer value
        /// </returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Marks the object as being an clean, 
        ///     which means not dirty.
        /// </summary>
        public virtual void MarkOld()
        {
            this.IsChanged = false;
            this.New = false;
        }

        /// <summary>
        /// Saves the object to the data store (inserts, updates or deletes).
        /// </summary>
        /// <returns>The SaveAction.</returns>
        public virtual SaveAction Save()
        {
            
            if (!this.Valid && !this.Deleted)
            {
               // throw new InvalidOperationException(this.ValidationMessage);
                return SaveAction.None;
            }

            if (this.Disposed && !this.Deleted)
            {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.InvariantCulture, "You cannot save a disposed {0}", this.GetType().Name));
            }

            return this.IsChanged ? this.Update() : SaveAction.None;
        }


        #region Implemented Interfaces

        #region IChangeTracking

        /// <summary>
        /// Resets the object's state to unchanged by accepting the modifications.
        /// </summary>
        public void AcceptChanges()
        {
            this.Save();
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Disposes the object and frees ressources for the Garbage Collector.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion

        /// <summary>
        /// Raises the Saved event.
        /// </summary>
        /// <param name="businessObject">
        /// The business Object.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected static void OnSaved(BaseModels<T, TKey> businessObject, SaveAction action)
        {
            if (Saved != null)
            {
                Saved(businessObject, new SavedEventArgs(action));
            }
        }

        /// <summary>
        /// Raises the Saving event
        /// </summary>
        /// <param name="businessObject">
        /// The business Object.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        protected static void OnSaving(BaseModels<T, TKey> businessObject, SaveAction action)
        {
            if (Saving != null)
            {
                Saving(businessObject, new SavedEventArgs(action));
            }
        }

        /// <summary>
        /// Add or remove a broken rule.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property.
        /// </param>
        /// <param name="errorMessage">
        /// The description of the error
        /// </param>
        /// <param name="isbroken">
        /// True if the validation rule is broken.
        /// </param>
        protected virtual void AddRule(string propertyName, string errorMessage, bool isbroken)
        {
            if (isbroken)
            {
                this._brokenRules[propertyName] = errorMessage;
            }
            else
            {
                this._brokenRules.Remove(propertyName);
            }
        }

        #region abstract CURD

        /// <summary>
        /// Deletes the object from the data store.
        /// </summary>
        protected abstract void DataDelete();

        /// <summary>
        /// Inserts a new object to the data store.
        /// </summary>
        protected abstract void DataInsert();

        /// <summary>
        /// Retrieves the object from the data store and populates it.
        /// </summary>
        /// <param name="id">
        /// The unique identifier of the object.
        /// </param>
        /// <returns>
        /// True if the object exists and is being populated successfully
        /// </returns>
        protected abstract T DataSelect(TKey id);

        /// <summary>
        /// Updates the object in its data store.
        /// </summary>
        protected abstract void DataUpdate();

        #endregion

        /// <summary>
        /// Disposes the object and frees ressources for the Garbage Collector.
        /// </summary>
        /// <param name="disposing">
        /// If true, the object gets disposed.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (this.Disposed)
                {
                    return;
                }

                if (!disposing)
                {
                    return;
                }
                
                this._brokenRules.Clear();
            }
            finally
            {
                this.Disposed = true;
            }
        }

      
        /// <summary>
        /// Reinforces the business rules by adding additional rules to the 
        ///     broken rules collection.
        /// </summary>
        protected abstract void ValidationRules();

        /// <summary>
        /// Is called by the save method when the object is old and dirty.
        /// </summary>
        /// <returns>
        /// The update.
        /// </returns>
        private SaveAction Update()
        {
            var action = SaveAction.None;

            if (this.Deleted)
            {
                if (!this.New)
                {
                    action = SaveAction.Delete;
                    OnSaving(this, action);
                    this.DataDelete();
                }
            }
            else
            {
                if (this.New)
                {
                    action = SaveAction.Insert;
                    OnSaving(this, action);
                    this.DataInsert();
                }
                else
                {
                    action = SaveAction.Update;
                    OnSaving(this, action);
                    this.DataUpdate();
                }

                this.MarkOld();
            }

            OnSaved(this, action);
            return action;
        }


    }
}
