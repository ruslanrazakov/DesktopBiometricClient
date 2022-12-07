using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Messenger.Client.MVVM
{
    public static class ObservableCollectionUtilities
    {
        /// <summary>
        /// Расширение ObservableCollection для возможности добавления массива элементов
        /// в коллекцию типа ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
    }
}
