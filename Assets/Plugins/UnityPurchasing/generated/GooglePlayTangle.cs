#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("xvcbpUEXQVIm6mM3LBUEaQ1foqcuMuS/GaK86fdAwitYzWckCrCt/Xnclx91WuSJmq9ZDs5e7rDMMPrcAhvWmNFMMLaWRc4B1S94QdpwvWDVF7l84PgytVDkAbWRpzIEK93hx0gskyxITIdBP95pRQWevAX4GBYUIhTDJItqZxpHH9fooLw0Cjr6OO9KIFstR/yUX6v1XAIQNTIZqvlrxkqYvuYlFBhZKgE2PNwBYeqFl6H/1Wfkx9Xo4+zPY61jEujk5OTg5ebzATaYOOInM74EWFafUiZH0RVZqcQivhkKk3DPBfmTQhlHS6ym8MbmX2WqKOpSgteFbkvJ3H5/1VHYirhn5Orl1Wfk7+dn5OTlb3pQ67+DjYbjK1evi8Qyqufm5OXk");
        private static int[] order = new int[] { 9,7,4,10,10,6,13,10,11,9,12,13,13,13,14 };
        private static int key = 229;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
