/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 01 мая 2026 06:52:27
 * Version: 1.0.22
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
