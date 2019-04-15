namespace MattEland.AI.Neural
{
    public class NeuronConnection
    {
        public NeuronConnection(Neuron source, Neuron target)
        {
            Source = source;
            Target = target;

            target.RegisterIncomingConnection(this);
        }

        public void Fire(decimal value)
        {
            Target.Receive(value * Weight);
        }

        public decimal Weight { get; set; } = 1;

        public Neuron Source { get; }

        public Neuron Target { get; }
    }
}