/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 30 мая 2026 15:43:07
 * Version: 1.0.51
 */

using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    public interface IFreelancerCollection
    {
        StateScanCollection GetCollection(string file, string nameHeader, string key);
    }
}
