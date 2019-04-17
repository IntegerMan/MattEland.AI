namespace MattEland.AI.Neural.Functional

/// Represents a connection between two Neurons
type NeuronConnection(source: Neuron, weight: decimal) = 
  member this.Source: Neuron = source;
  member this.Weight: decimal = weight;
  member this.Calculate = this.Weight * this.Source.Value;

/// Represents a node in a Neural Network
and Neuron () =

  let mutable connections = []
  let mutable value = 0M;

  /// Incoming connections from other Neurons (if any)
  member this.Inputs = connections

  /// Exposes the current calculated amount of the Neuron
  member this.Value
    with get () = value
    and set (newValue) = value <- newValue

  /// Adds an incoming connection from another Neuron
  member this.AddIncomingConnection c = connections <- c :: connections

  /// Adds all connections together, stores the result in Value, and returns the value
  member this.Evaluate(): decimal =
    value <- List.sumBy (fun (c:NeuronConnection) -> c.Calculate) this.Inputs;
    value;
