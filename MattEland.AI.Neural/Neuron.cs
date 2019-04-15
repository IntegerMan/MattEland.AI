using System.Collections.Generic;
using System.Linq;

namespace MattEland.AI.Neural
{
    public class Neuron
    {
        private readonly IList<NeuronConnection> _outgoing;
        private readonly IList<NeuronConnection> _incoming;
        private decimal _sum;

        public decimal Value { get; set; }

        public Neuron()
        {
            _incoming = new List<NeuronConnection>();
            _outgoing = new List<NeuronConnection>();
        }

        public void ConnectTo(Neuron nextNeuron)
        {
            _outgoing.Add(new NeuronConnection(this, nextNeuron));
        }

        public void Evaluate()
        {
            if (_incoming.Any())
            {
                Value = _sum / _incoming.Count;
                _sum = 0;
            }

            if (ShouldFire)
            {
                foreach (var conn in _outgoing)
                {
                    conn.Fire(Value);
                }
            }
        }

        public bool ShouldFire => true;
        public IList<NeuronConnection> OutgoingConnections => _outgoing;
        public IList<NeuronConnection> IncomingConnections => _incoming;

        public void Receive(decimal value)
        {
            _sum += value;
        }

        public void RegisterIncomingConnection(NeuronConnection neuronConnection)
        {
            _incoming.Add(neuronConnection);
        }
    }
}