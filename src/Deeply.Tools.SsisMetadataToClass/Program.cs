#region Copyright (c) 2013 James Snape
// <copyright file="Program.cs" company="James Snape">
//  Copyright 2013 James Snape
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// </copyright>
#endregion

namespace Deeply.Tools.SsisMetadataToClass
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Args.Help.Formatters;

    /// <summary>
    /// Main program entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Gets the global options.
        /// </summary>
        public static Options Options { get; private set; }

        /// <summary>
        /// Main program entry point.
        /// </summary>
        /// <param name="args">Arguments passed to the command line.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Program is shutting down anyway.")]
        [STAThreadAttribute]
        internal static void Main(string[] args)
        {
            Program.Options = ParseArguments(args);

            if (Program.Options.Help)
            {
                return;
            }

            var writer = CreateOutput(Program.Options);

            try
            {
                var text = LoadSourceText(Program.Options);
                var metadata = MetadataParser.Parse(text);

                ClassWriter.Write(Program.Options, metadata, writer);
            }
            catch (FormatException)
            {
                Console.WriteLine("Input does not contain text in the correct format.");
            }
            finally
            {
                if (writer != Console.Out)
                {
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Depending on the current settings fetches the source metadata.
        /// </summary>
        /// <param name="options">Load options.</param>
        /// <returns>A string representing the whole metadata input.</returns>
        private static string LoadSourceText(Options options)
        {
            switch (options.MetadataSource)
            {
                case "CONS":
                    return LoadSourceTextFromConsole();
                case "CLIP":
                    return Clipboard.GetText();
                default:
                    return File.ReadAllText(options.MetadataSource);
            }
        }

        /// <summary>
        /// Reads the entire console input.
        /// </summary>
        /// <returns>A string representing the entire console input.</returns>
        private static string LoadSourceTextFromConsole()
        {
            StringBuilder text = new StringBuilder();
            string line = null;

            while ((line = Console.ReadLine()) != null)
            {
                text.AppendLine(line);
            }

            return text.ToString();
        }

        /// <summary>
        /// Creates the correct output.
        /// </summary>
        /// <param name="options">Save options.</param>
        /// <returns>A text writer instance.</returns>
        private static TextWriter CreateOutput(Options options)
        {
            switch (options.OutputFile)
            {
                case "CONS":
                    return Console.Out;
                default:
                    return new StreamWriter(options.OutputFile);
            }
        }

        /// <summary>
        /// Parses the command line arguments into 
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        /// <returns>A set of options.</returns>
        private static Options ParseArguments(string[] args)
        {
            var optionModel = Args.Configuration.Configure<Options>();
            var options = optionModel.CreateAndBind(args);

            if (options.Help)
            {
                var help = new Args.Help.HelpProvider().GenerateModelHelp(optionModel);
                var formatter = new ConsoleHelpFormatter();

                Console.WriteLine(formatter.GetHelp(help));
            }

            return options;
        }
    }
}
