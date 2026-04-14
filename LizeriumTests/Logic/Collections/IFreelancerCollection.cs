/*
 * Author: Nikolay Dvurechensky and Librelancer Contributors
 * Site: https://dvurechensky.pro/
 * Gmail: dvurechenskysoft@gmail.com
 * Last Updated: 15 апреля 2026 01:09:28
 * Version: 1.0.3
 */

using LizeriumTests.Components.Collections;

namespace LizeriumTests.Logic.Collections
{
    public interface IFreelancerCollection
    {
        StateScanCollection GetCollection(string file, string nameHeader, string key);
    }
}
