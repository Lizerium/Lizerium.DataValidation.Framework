/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 апреля 2026 08:11:04
 * Version: 1.0.15
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
