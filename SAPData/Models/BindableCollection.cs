using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPData.Models
{
    public class BindableCollection<T> : ObservableCollection<T>, ITypedList
    {
        string ITypedList.GetListName(PropertyDescriptor[] listAccessors) { return null; }
        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return TypeDescriptor.GetProperties(typeof(T), PropertyFilter);
        }
        static readonly Attribute[] PropertyFilter = { BrowsableAttribute.Yes };

        public void AddRange(IEnumerable<T> collection)
        {
            collection.ToList().ForEach(x => base.Add(x));
        }
    }
}
