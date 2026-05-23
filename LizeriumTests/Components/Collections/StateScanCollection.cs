/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 23 мая 2026 11:13:37
 * Version: 1.0.44
 */

using LizeriumTests.Logic.Collections;

namespace LizeriumTests.Components.Collections
{
    public class StateScanCollection
    {
        public StateScanCollectionEnum State {  get; set; } = StateScanCollectionEnum.Success;
        public IFreelancerCollection Collection { get; set; }
        public string Message {  get; set; } = string.Empty;

        public StateScanCollection()
        {
        }

        public StateScanCollection(StateScanCollectionEnum error, 
            IFreelancerCollection collection, 
            string message)
        {
            State = error;
            Collection = collection;
            Message = message;
        }
    }
}
