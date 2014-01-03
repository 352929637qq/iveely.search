﻿/*==========================================
 *创建人：刘凡平
 *邮  箱：liufanping@iveely.com
 *电  话：13896622743
 *版  本：0.1.0
 *Iveely=I void everything,except love you!
 *========================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SharpICTCLAS;

namespace Iveely.Framework.Algorithm.AI
{
    /// <summary>
    /// 句子解读器
    /// </summary>
    public class SentenceGetter
    {
        /// <summary>
        /// 句子中词汇特征提取规则
        /// </summary>
        internal class Extractor
        {
            /// <summary>
            /// 词汇基本词性标注
            /// </summary>
            public HashSet<string> Signs { get; set; }

            /// <summary>
            /// 允许接着往下连接的词性标注
            /// </summary>
            public HashSet<string> NextContinueSigns { get; set; }

            /// <summary>
            /// 前面允许存在的词性标注
            /// </summary>
            public HashSet<string> FrontAllowSigns { get; set; }

            /// <summary>
            /// 后面不允许出现的词性标注
            /// </summary>
            public HashSet<string> NextBanSigns { get; set; }

            /// <summary>
            /// 前面不允许出现的词性标注
            /// </summary>
            public HashSet<string> FrontBanSigns { get; set; }

            public Extractor()
            {
                Signs = new HashSet<string>();
                NextContinueSigns = new HashSet<string>();
                FrontAllowSigns = new HashSet<string>();
                NextBanSigns = new HashSet<string>();
                FrontBanSigns = new HashSet<string>();
                InitSigns();
            }

            /// <summary>
            /// 初始化标注
            /// </summary>
            public virtual void InitSigns()
            {

            }
        }

        /// <summary>
        /// 人名提取器
        /// </summary>
        internal class NameExtractor : Extractor
        {
            public override void InitSigns()
            {
                //    HashSet<string> signs = new HashSet<string>();
                Signs.Add("nr");
                Signs.Add("nr1");
                Signs.Add("nr2");
                Signs.Add("nrj");
                Signs.Add("nrf");
                Signs.Add("nz");


                NextBanSigns.Add("n");

                FrontAllowSigns.Add("no-type");
                FrontAllowSigns.Add("v");
                FrontAllowSigns.Add("w");
                FrontAllowSigns.Add("p");
                FrontAllowSigns.Add("n");
            }
        }

        /// <summary>
        /// 地名提取器
        /// </summary>
        internal class LocationExtractor : Extractor
        {
            public override void InitSigns()
            {
                //寻找的词性
                Signs.Add("ns");
                Signs.Add("nsf");

                //后面禁止出现的词性
                NextBanSigns.Add("p");

                //后面允许出现的词性
                NextContinueSigns.Add("n");
                NextContinueSigns.Add("ns");
                //NextContinueSigns.Add("p");

                //前面允许出现的词性
                FrontAllowSigns.Add("p");
                FrontAllowSigns.Add("w");
                FrontAllowSigns.Add("ns");
                FrontAllowSigns.Add("nsf");
                FrontAllowSigns.Add("no-type");
                FrontAllowSigns.Add("b");
                FrontAllowSigns.Add("uj");
                FrontAllowSigns.Add("v");
                FrontAllowSigns.Add("f");
            }
        }

        /// <summary>
        /// 机构提取器
        /// </summary>
        internal class AgencyExtrator : Extractor
        {
            public override void InitSigns()
            {
                //寻找的词性
                Signs.Add("nt");
                Signs.Add("nz");
                Signs.Add("nl");
                Signs.Add("nrj");
                Signs.Add("ng");

                //后面不允许出现的词性
                NextBanSigns.Add("n");

                //前面允许出现的词性
                FrontAllowSigns.Add("no-type");
                FrontAllowSigns.Add("v");
            }
        }

        /// <summary>
        /// 事件提取器
        /// </summary>
        internal class EventExtrator : Extractor
        {
            public override void InitSigns()
            {
                //添加寻找的词性
                Signs.Add("n");
                Signs.Add("j");

                //后面不允许存在的词性
                NextBanSigns.Add("d");
                NextBanSigns.Add("v");
                NextBanSigns.Add("w");
                NextBanSigns.Add("uj");
                NextBanSigns.Add("ad");
                NextBanSigns.Add("m");
                NextBanSigns.Add("f");
                NextBanSigns.Add("c");
                NextBanSigns.Add("w");

                //前面允许出现的词性
                FrontAllowSigns.Add("v");
                FrontAllowSigns.Add("j");
                FrontAllowSigns.Add("ng");
                FrontAllowSigns.Add("no-type");

                //后面允许的词性
                NextContinueSigns.Add("n");
                NextContinueSigns.Add("j");
                NextContinueSigns.Add("vn");
            }
        }

        /// <summary>
        /// 时间提取器
        /// </summary>
        internal class TimeExtrator : Extractor
        {
            public override void InitSigns()
            {
                //添加寻找的词性标注
                Signs.Add("t");
                Signs.Add("tg");

                //后面不允许出现的词性
                NextBanSigns.Add("ns");
                NextBanSigns.Add("u");

                //后面允许继续出现的词性
                NextContinueSigns.Add("t");
                NextContinueSigns.Add("tg");
                NextContinueSigns.Add("f");
                //NextContinueSigns.Add("m");
                NextContinueSigns.Add("q");

                //前面允许出现的词性
                FrontAllowSigns.Add("v");
                FrontAllowSigns.Add("p");
                FrontAllowSigns.Add("w");
                FrontAllowSigns.Add("no-type");
                FrontAllowSigns.Add("m");

            }
        }

        /// <summary>
        /// 获取人名
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public string[] GetNames(List<WordResult[]> words)
        {
            return GetValuesByRules(words, new NameExtractor());
        }

        /// <summary>
        /// 获取地名
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public string[] GetLocations(List<WordResult[]> words)
        {
            return GetValuesByRules(words, new LocationExtractor());
        }

        /// <summary>
        /// 获取机构
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public string[] GetAgency(List<WordResult[]> words)
        {
            return GetValuesByRules(words, new AgencyExtrator());
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public string[] GetTime(List<WordResult[]> words)
        {
            return GetValuesByRules(words, new TimeExtrator());
        }

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        public string[] GetEvent(List<WordResult[]> words)
        {
            return GetValuesByRules(words, new EventExtrator());
        }

        private string[] GetValuesByRules(List<WordResult[]> words, Extractor extractor)
        {
            HashSet<string> bodys = new HashSet<string>();
            string answer = string.Empty;
            bool shoudContine = false;
            string lastType = "no-type";

            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 1; j < words[i].Length - 1; j++)
                {
                    string type = Utility.GetPOSString(words[i][j].nPOS).Trim();
                    if ((extractor.Signs.Contains(type) && extractor.FrontAllowSigns.Contains(lastType)) || shoudContine)
                    {
                        if (j < words[i].Length - 2)
                        {
                            string nextType = Utility.GetPOSString(words[i][j + 1].nPOS);

                            if (extractor.NextContinueSigns.Contains(nextType))
                            {
                                if (!shoudContine)
                                {
                                    lastType = type;
                                }
                                answer += words[i][j].sWord;
                                shoudContine = true;
                                continue;
                            }
                            if (!extractor.NextBanSigns.Contains(nextType))
                            {
                                answer += words[i][j].sWord;
                                if (answer != string.Empty && !bodys.Contains(answer))
                                {
                                    bodys.Add(answer);
                                    answer = string.Empty;
                                    shoudContine = false;
                                    continue;
                                }
                            }
                            else
                            {
                                if (answer != string.Empty && !bodys.Contains(answer))
                                {
                                    bodys.Add(answer);
                                    answer = string.Empty;
                                    shoudContine = false;
                                }
                                continue;
                            }
                        }
                        shoudContine = false;
                        answer += words[i][j].sWord;
                        if (answer != string.Empty && !bodys.Contains(answer))
                        {
                            bodys.Add(answer);
                            answer = string.Empty;
                            continue;
                        }
                    }
                    if (answer != string.Empty && !bodys.Contains(answer))
                    {
                        bodys.Add(answer);
                        answer = string.Empty;
                    }

                    shoudContine = false;
                    lastType = type;

                }
            }
            return bodys.ToArray();
        }
    }
}
