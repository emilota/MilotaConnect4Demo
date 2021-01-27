// Created and programmed by Eric Milota, 2021

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public static class Util
    {
        private static Int64 gTicksFirstCall = 0;

        public static Int64 GetMS()
        {
            if (gTicksFirstCall == 0)
            {
                gTicksFirstCall = DateTime.UtcNow.Ticks - TimeSpan.TicksPerMillisecond;
            }
            return (DateTime.UtcNow.Ticks - gTicksFirstCall) / TimeSpan.TicksPerMillisecond;
        }

        public static int GetRandomRange(int min, int max) // min/max INCLUSIVE
        {
            if (min > max)
            {
                int tmp = max;
                max = min;
                min = tmp;
            }
            int range = max - min + 1;
            if (range < 1)
                range = 1;

            System.Random random = new System.Random(DateTime.Now.Millisecond);
            int num = (random.Next(0, range) % (range));
            int returnValue = min + num;
            return returnValue;
        }

        public static bool Blink(int ms)
        {
            return ((Util.GetMS() % (ms*2)) < ms);
        }

        public static void ShuffleList<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Util.GetRandomRange(0, n);
                T value = list[k];
                list[ k ] = list[ n ];
                list[ n ] = value;
            }
        }

        public static string GetIntListString(List<int> intList)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (intList == null)
            {
                stringBuilder.Append("null");
            }
            else
            {
                stringBuilder.Append("[");
                for(int index = 0; index < intList.Count; index++)
                {
                    if (index > 0)
                        stringBuilder.Append(",");
                    stringBuilder.Append(Convert.ToString(intList[ index ]));
                }
                stringBuilder.Append("]");
            }
            return stringBuilder.ToString();

        }

        public static void SetGameObjectTextMeshProText(GameObject gameObject, string message)
        {
            if (gameObject == null)
                return; // error!
            message = message ?? "";

            // there can be two different kinds of text objects...let's try them both
            TMPro.TextMeshPro text = gameObject.GetComponent<TMPro.TextMeshPro>();
            if (text == null)
            {
                TMPro.TextMeshProUGUI text2 = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
                if (text2 != null)
                {
                    text2.text = message;
                }
            }
            else
            {
                text.text = message;
            }
        }
    }
}
