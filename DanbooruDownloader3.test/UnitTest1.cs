﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DanbooruDownloader3.DAO;
using DanbooruDownloader3.Engine;
using System.Xml;
using DanbooruDownloader3.Entity;
using System.Xml.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace DanbooruDownloader3.test
{
    [TestClass]
    public class UnitTest1
    {
        string sourceProvider = @"../../../DanbooruDownloader3.test/DanbooruProviderList.xml";
        string sourceDanbooruXml = @"../../../DanbooruDownloader3.test/TestXml/danbooru.xml";
        string sourceYandereXml = @"../../../DanbooruDownloader3.test/TestXml/yande.re.xml";

        string sourceDanbooruTagsXml = @"../../../DanbooruDownloader3.test/TestXml/tags_danbooru.xml";
        string sourceYandereTagsXml = @"../../../DanbooruDownloader3.test/TestXml/tags_yande.re.xml";

        [TestMethod]
        public void TestShimmie2Parser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            var shimmie2Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Shimmie2).First<DanbooruProvider>();
            XmlReader reader = new XmlTextReader(@"../../../DanbooruDownloader3.test/TestXml/shimmie2.xml");

            var list = ShimmieEngine.ParseRSS(reader, shimmie2Provider, "", "", null);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 9);
        }

        [TestMethod]
        public void TestProviderSave()
        {            
            string target = @"../../../DanbooruDownloader3.test/testSave.xml";
            DanbooruProviderDao pd = new DanbooruProviderDao();
            var list = pd.Read(sourceProvider);
            list[0].Name = "hahaha";
            pd.Save(list, target);
            Assert.IsTrue(System.IO.File.Exists(target));
            XDocument doc = XDocument.Load(target);
            list = pd.Read(target);
            Assert.IsTrue(list[0].Name == "hahaha");
        }

        [TestMethod]
        public void TestDanbooruEngineParser()
        {
            DanbooruProviderDao pd = new DanbooruProviderDao();
            DanbooruXmlEngine e = new DanbooruXmlEngine();

            {
                XDocument doc = XDocument.Load(sourceDanbooruXml);
                var searchQuery = new DanbooruSearchParam();
                searchQuery.Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Danbooru && x.Name.Contains("danbooru")).First<DanbooruProvider>();
                BindingList<DanbooruPost> result = e.Parse(doc.ToString(), searchQuery);
                Assert.IsNotNull(result);
                Assert.IsNotNull(e.RawData);
                Assert.IsTrue(e.TotalPost == 1021107);
                Assert.IsTrue(result.Count == 20);
                Assert.IsTrue(result[0].PreviewUrl == "http://danbooru.donmai.us/ssd/data/preview/73531fc4dda535ef87e57df633caf756.jpg");
            }

            {
                XDocument doc = XDocument.Load(sourceYandereXml);
                var searchQuery = new DanbooruSearchParam();
                searchQuery.Provider = pd.Read(sourceProvider).Where<DanbooruProvider>(x => x.BoardType == BoardType.Danbooru && x.Name.Contains("yande.re")).First<DanbooruProvider>();
                BindingList<DanbooruPost> result = e.Parse(doc.ToString(), searchQuery);
                Assert.IsNotNull(result);
                Assert.IsNotNull(e.RawData);
                Assert.IsTrue(e.TotalPost == 160753);
                Assert.IsTrue(result.Count == 16);
                Assert.IsTrue(result[0].PreviewUrl == "https://yande.re/data/preview/d3/41/d34184030ee19c6e63051967cf135f65.jpg");
            }
        }

        [TestMethod]
        public void TestDanbooruTags()
        {
            {
                XmlSerializer ser = new XmlSerializer(typeof(DanbooruTagCollection));
                DanbooruTagCollection tags = (DanbooruTagCollection)ser.Deserialize(File.OpenText(sourceDanbooruTagsXml));

                Assert.IsTrue(tags.Tag.Length == 151190);
                Assert.IsTrue(tags.Tag[0].Name == "geordi_la_forge");
                Assert.IsTrue(tags.Tag[0].Ambiguous == false);
                Assert.IsTrue(tags.Tag[0].Type == DanbooruTagType.Character);
                Assert.IsTrue(tags.Tag[0].Count == 1);
                Assert.IsTrue(tags.Tag[0].Id == "525178");

                var AmbiguousTags = tags.Tag.First<DanbooruTag>(x => x.Id == "1723");
                Assert.IsTrue(AmbiguousTags.Name == "parody");
                Assert.IsTrue(AmbiguousTags.Type == DanbooruTagType.General);
                Assert.IsTrue(AmbiguousTags.Count == 16546);
                Assert.IsTrue(AmbiguousTags.Ambiguous == true);

                Assert.IsTrue(tags.GeneralTag.Length == 23011);
                Assert.IsTrue(tags.ArtistTag.Length == 67705);
                Assert.IsTrue(tags.CopyrightTag.Length == 12209);
                Assert.IsTrue(tags.CharacterTag.Length == 48265);
                Assert.IsTrue(tags.CircleTag.Length == 0);
                Assert.IsTrue(tags.FaultsTag.Length == 0);

                Assert.IsTrue(tags.GeneralTag.Length + tags.ArtistTag.Length + tags.CopyrightTag.Length + tags.CharacterTag.Length + tags.CircleTag.Length + tags.FaultsTag.Length == tags.Tag.Length);
            }
        }

        [TestMethod]
        public void TestDanbooruTagsDao()
        {
            {
                var dao = new DanbooruTagsDao(sourceDanbooruTagsXml);

                Assert.IsTrue(dao.IsArtistTag("raistlinkid"));
                Assert.IsTrue(dao.IsCopyrightTag("i_feel_fine"));
                Assert.IsTrue(dao.IsCharacterTag("geordi_la_forge"));
                //Assert.IsTrue(dao.IsCircleTag(""));
                //Assert.IsTrue(dao.IsFaultsTag(""));
                Assert.IsTrue(dao.GetTagType("cracking_knuckles") == DanbooruTagType.General);
                Assert.IsTrue(dao.GetTagType("unknown_tags!!!@!@#!") == DanbooruTagType.General);
            }
        }

        [TestMethod]
        public void TestJsonDecode()
        {
            string input = "Here could be characters like \u00e5\u00e4\u00f6\u041c\u043e\u0439";
            string expected = "Here could be characters like åäöМой";
            Assert.AreEqual(Helper.DecodeEncodedNonAsciiCharacters(input), expected);
        }

        [TestMethod]
        public void TestDownloadTagsXml()
        {
            string url = @"https://yande.re/tag/index.xml?limit=0";
            string filename = @"test-tag.xml";
            DanbooruDownloader3.Utils.ExtendedWebClient client = new Utils.ExtendedWebClient();

            client.DownloadFile(url, filename);
            Assert.IsTrue(File.Exists(filename));
        }

        [TestMethod]
        public void TestMergeTagsXml()
        {
            string source = @"../../../DanbooruDownloader3.test/TestXml/tags-source.xml";
            string actualTarget = @"../../../DanbooruDownloader3.test/TestXml/tags-target.xml";
            string target = "tags-target.xml";

            File.Copy(actualTarget, target);
            var message = DanbooruTagsDao.Merge(source, target);
            var targetInstance = new DanbooruTagsDao(target);

            Assert.IsTrue(targetInstance.Tags.Tag.Length > 0);
        }

        [TestMethod]
        public void TestCheckUri()
        {
            string url = "http://chan.sankakustatic.com/data/e9/49/e9496f78362ca9748f208f128f56ef32.jpg";
            Uri uri = null;

            Assert.IsTrue(Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri));
            Assert.IsTrue(uri.ToString() == url);
        }
    }
}
