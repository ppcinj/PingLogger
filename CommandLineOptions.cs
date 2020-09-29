using CommandLine;

namespace PingLogger
{
    class CommandLineOptions
    {
        [Option('o', "outputFile", Required = true, HelpText = "Where should the Log be written to?")]
        public string OutputFile { get; set; }

        [Option('t', "target", Required = true, HelpText = "Which host to ping?")]
        public string PingTarget { get; set; }

        [Option("timeout", Required = false, HelpText = "Custom Timeout in ms. Default = 1000")]
        public int? Timeout { get; set; }
    }
}
