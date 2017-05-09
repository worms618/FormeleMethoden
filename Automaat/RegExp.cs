﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaat
{
    public class CompareByLength : IComparer<string>
    {
        public int Compare(string y, string x)
        {
            String s1 = (string)y;
            String s2 = (string)x;
            if (s1 == null || s2 == null) return -1;

            if (s1.Count() == s2.Count())
            { return s1.CompareTo(s2); }
            else
            { return s1.Count() - s2.Count(); }
        }
    }


    public class RegExp
    {
        // De mogelijke operatoren voor een reguliere expressie (+, *, |, .) 
        // Daarnaast ook een operator definitie voor 1 keer repeteren (default)
        public enum Operator { PLUS, STAR, OR, DOT, ONE }

        RegExp left;
        RegExp right;

        Operator _operator;
        String terminals;        

        public RegExp()
        {
            _operator = Operator.ONE;
            terminals = "";
            left = null;
            right = null;
        }

        public RegExp(String p)
        {
            _operator = Operator.ONE;
            terminals = p;
            left = null;
            right = null;
        }

        public RegExp plus()
        {
            RegExp result = new RegExp();
            result._operator = Operator.PLUS;
            result.left = this;
            return result;
        }

        public RegExp star()
        {
            RegExp result = new RegExp();
            result._operator = Operator.STAR;
            result.left = this;
            return result;
        }

        public RegExp or(RegExp e2)
        {
            RegExp result = new RegExp();
            result._operator = Operator.OR;
            result.left = this;
            result.right = e2;
            return result;
        }

        public RegExp dot(RegExp e2)
        {
            RegExp result = new RegExp();
            result._operator = Operator.DOT;
            result.left = this;
            result.right = e2;
            return result;
        }

        public SortedSet<String> getLanguage(int maxSteps)
        {
            //compare by lenght?
            SortedSet<String> emptyLanguage = new SortedSet<string>( new CompareByLength());
            SortedSet<String> languageResult = new SortedSet<string>( new CompareByLength());

            SortedSet<String> languageLeft, languageRight;

            if (maxSteps < 1) return emptyLanguage;

            switch (this._operator) {
                case Operator.ONE:
                languageResult.Add(terminals);
                break;

                case Operator.OR:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                languageRight = right == null ? emptyLanguage : right.getLanguage(maxSteps - 1);
                foreach(var l in languageLeft)
                        languageResult.Add(l);

                foreach (var r in languageRight)
                        languageResult.Add(r);
                break;
                

                case Operator.DOT:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                languageRight = right == null ? emptyLanguage : right.getLanguage(maxSteps - 1);
                foreach (var s1 in languageLeft)
                    foreach (var s2 in languageRight)
                    { languageResult.Add(s1 + s2); }
                break;

                // STAR(*) en PLUS(+) kunnen we bijna op dezelfde manier uitwerken:
                case Operator.STAR:
                case Operator.PLUS:
                languageLeft = left == null ? emptyLanguage : left.getLanguage(maxSteps - 1);
                foreach (var l in languageLeft)
                        languageResult.Add(l);
                for (int i = 1; i < maxSteps; i++)
                {
                    HashSet<String> languageTemp = new HashSet<String>(languageResult);
                    foreach (var  s1 in languageLeft)
                    {
                        foreach (var s2 in languageTemp)
                        {
                            languageResult.Add(s1 + s2);
                        }
                    }
                }
                if (this._operator  == Operator.STAR)
                        { languageResult.Add(""); }
                break;

                default:
                   Console.WriteLine("getLanguage is nog niet gedefinieerd voor de operator: " + this._operator);
                break;
            }


            return languageResult;
        }

    }
}