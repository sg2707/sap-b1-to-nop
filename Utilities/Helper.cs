using System;
using System.Text;
using System.Reflection;
using System.ComponentModel;
//using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
namespace Utilities
{
    public class Helper
    {


        public static StringBuilder getAllExceptionMessages(Exception ex)
        {
            StringBuilder innerEx = new StringBuilder();
            while (ex.InnerException != null)
            {
                innerEx.Append((ex.Message ?? "") + " > ");
                ex = ex.InnerException;
            }
            innerEx.AppendLine((ex.Message ?? ""));
            return innerEx;
        }

        /// <summary>
        /// Get related display name
        /// </summary>
        /// <param name="descriptor">propertInfo/PropertyDescriptor of the field whose display name should be returned</param>
        /// <returns>Vaue for display name. If no value returns null</returns>
        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Get readonly status
        /// </summary>
        /// <param name="descriptor">propertInfo/PropertyDescriptor of the field whose display name should be returned</param>
        /// <returns>Value for readonly property. If not available, returns true.</returns>
        public static bool GetPropertyReadOnly(object descriptor)
        {
            bool returnval = true;
            var pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var readOnlyAttrib = pd.Attributes[typeof(ReadOnlyAttribute)] as ReadOnlyAttribute;
                if (readOnlyAttrib != null && readOnlyAttrib != ReadOnlyAttribute.Default)
                {
                    returnval = readOnlyAttrib.IsReadOnly;
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(ReadOnlyAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var readOnlyAttrib = attributes[i] as ReadOnlyAttribute;
                        if (readOnlyAttrib != null && readOnlyAttrib != ReadOnlyAttribute.Default)
                        {
                            returnval = readOnlyAttrib.IsReadOnly;
                        }
                    }
                }
            }
            return returnval;
        }
        /// <summary>
        /// Get DisplayFormat 
        /// </summary>
        /// <param name="descriptor">propertInfo/PropertyDescriptor of the field whose display name should be returned</param>
        /// <returns>Value for DisplayFormat > DataFormatString property. If not available, returns true.</returns>
        public static string GetPropertyDisplayFormat(object descriptor)
        {
            string returnval = null;
            var pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var DisplayFormatAttrib = pd.Attributes[typeof(DisplayFormatAttribute)] as DisplayFormatAttribute;
                if (DisplayFormatAttrib != null)
                {
                    returnval = DisplayFormatAttrib.DataFormatString;
                }
            }
            else
            {
                var pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayFormatAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var readOnlyAttrib = attributes[i] as DisplayFormatAttribute;
                        if (readOnlyAttrib != null)
                        {
                            returnval = readOnlyAttrib.DataFormatString;
                        }
                    }
                }
            }
            return returnval;
        }
        //public static BindCommandAttributes GetPropertyBindCommand(object descriptor)
        //{
        //    var _pd = descriptor as PropertyDescriptor;
        //    if (_pd != null)
        //    {
        //        // Check for BindCommand attribute and set the Binding Command accordingly
        //        var _command = _pd.Attributes[typeof(BindCommandAttributes)] as BindCommandAttributes;
        //        if (_command != null)
        //            return _command;
        //    }
        //    return new BindCommandAttributes();
        //}
        //public static string GetValidationErrors(DbEntityValidationException ex)
        //{
        //    StringBuilder validationErrors = new StringBuilder();
        //    foreach (var item in ex.EntityValidationErrors)
        //    {
        //        foreach (var validationError in item.ValidationErrors)
        //        {
        //            validationErrors.AppendLine(string.Format("{0} - {1}", validationError.PropertyName, validationError.ErrorMessage));
        //        }
        //    }
        //    return validationErrors.ToString();
        //}
        public static string GetExceptionDetail(Exception ex, bool withTrace = false)
        {
            StringBuilder exceptionDetail = new StringBuilder();
            if (withTrace)
                exceptionDetail.AppendLine(string.Format("{0} {1}", (ex.Message ?? string.Empty), ex.StackTrace));
            else
                exceptionDetail.Append($"{ ex.Message ?? string.Empty}. ");
            if (ex.InnerException != null)
                exceptionDetail.Append(GetExceptionDetail(ex.InnerException));
            return exceptionDetail.ToString();
        }
        public static string GetExceptionDetail(AggregateException ex)
        {
            StringBuilder exceptionDetail = new StringBuilder();
            foreach (var item in ex.InnerExceptions)
            {
                exceptionDetail.Append(GetExceptionDetail(item));
            }
            return exceptionDetail.ToString();
        }
    }
}