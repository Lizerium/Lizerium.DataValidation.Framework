/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 16 мая 2026 10:47:11
 * Version: 1.0.37
 */

using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    public interface IFreelancerCollection
    {
        StateScanCollection GetCollection(string file, string nameHeader, string key);
    }
}
