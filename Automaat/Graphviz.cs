﻿using System;
using System.Collections.Generic;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace Automaat
{
    public class Graphviz
    {
        public static void PrintGraph(Automaat<string> data, string filename)
        {
            string s = "digraph{ ";
            s += "node [shape = doublecircle];";
                        
            foreach (string t in data._finalStates)
            {
                s += " " + ("S" + t) + " ";
            }
            s += ";  ";
            s += "node [shape = circle];";

            foreach (Transition<string> t in data._transitions)
            {
                s += " " + ("S" + t.FromState) + " -> " + ("S" + t.ToState) + " " + "[ label = " + "\"" + t.Symbol + "\"" + " ];";
            }
            s += " }";

            Console.WriteLine(s);

            GenerateGraphFile(s, filename);
        }

        static void GenerateGraphFile(string data, string filename)
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand =
                new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);

            var wrapper = new GraphGeneration(getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand);

            byte[] output = wrapper.GenerateGraph("digraph{ a->b; b->c; c->a; }", Enums.GraphReturnType.Jpg);
            System.IO.File.WriteAllBytes(filename + ".jpg", output);
        }
    }
}