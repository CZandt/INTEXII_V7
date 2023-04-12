using Microsoft.ML.OnnxRuntime.Tensors;

namespace Identity.Models.Onnx
{
    public class MummyData
    {
        public float squarenorthsouth { get; set; }
        public float depth { get; set; }
        public float southtohead { get; set; }
        public float squareeastwest { get; set; }
        public float westtohead { get; set; }
        public float length { get; set; }
        public float westtofeet { get; set; }
        public float southtofeet { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                squarenorthsouth, depth, southtohead, squareeastwest, westtohead, length, westtofeet, southtofeet
            };

            int[] dimensions = new int[] { 1, 8 };

            return new DenseTensor<float>(data, dimensions);
        }
    }
}
