/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 01 июня 2026 14:47:08
 * Version: 1.0.53
 */

using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    public interface IFreelancerCollection
    {
        StateScanCollection GetCollection(string file, string nameHeader, string key);
    }
}
