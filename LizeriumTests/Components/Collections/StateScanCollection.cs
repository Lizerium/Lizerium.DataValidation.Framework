/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 02 июля 2026 07:14:25
 * Version: 1.0.84
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
