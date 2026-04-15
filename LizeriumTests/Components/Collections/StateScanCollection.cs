/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 15 апреля 2026 06:52:09
 * Version: 1.0.5
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
