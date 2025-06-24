using ProjectTest.DataModels;

using Service.Const;

using static Core.Utils.JsonFileUtils;

namespace Test.DataProvider
{
    public class GetAccountDetailDataProvider
    {
         private static Dictionary<string, GetAccountDetailData> _accDetailData;

        public static Dictionary<string, GetAccountDetailData> LoadGetAccountDetailDataFile()
        {
            if (_accDetailData == null || _accDetailData.Count == 0)
            {
                _accDetailData = JsonFileUltils.ReadJsonAndParse<Dictionary<string, GetAccountDetailData>>(
                    FilePathConst.GET_ACC_DETAIL_DATA);
            }
            return _accDetailData;
        }
        public static IEnumerable<GetAccountDetailData> GetData(string key)
        {
            yield return LoadGetAccountDetailDataFile()[key];
        }
        public static IEnumerable<GetAccountDetailData> GetDetailValid()
        {
            yield return LoadGetAccountDetailDataFile()[TestDataKeyConst.GET_ACC_DETAIL_SUCCESS];
        }
        public static IEnumerable<GetAccountDetailData> GetDetailUserNotFound()
        {
            yield return LoadGetAccountDetailDataFile()[TestDataKeyConst.GET_ACC_DETAIL_NOTFOUND_USER];
        }
        public static IEnumerable<GetAccountDetailData> GetDetailDifferentUser()
        {
            yield return LoadGetAccountDetailDataFile()[TestDataKeyConst.GET_ACC_DETAIL_DIFFERENT_USER];
        }
        public static IEnumerable<GetAccountDetailData> GetDetailNoAuthen()
        {
            yield return LoadGetAccountDetailDataFile()[TestDataKeyConst.GET_ACC_DETAIL_NO_AUTHEN];
        }
    }
}