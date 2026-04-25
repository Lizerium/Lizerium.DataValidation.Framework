/*
 * Author: Nikolay Dvurechensky
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 25 апреля 2026 08:11:04
 * Version: 1.0.15
 */

using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    public interface IFreelancerCollection
    {
        StateScanCollection GetCollection(string file, string nameHeader, string key);
    }
}
