namespace NNLibrary
{
    public struct NetworkStruct
    {
        public string[][] Shapes { get; set; }
        public float[][][] Weights { get; set; }
        public float[][] Biases { get; set; }
        public string[] Activations { get; set; }

    }
}
