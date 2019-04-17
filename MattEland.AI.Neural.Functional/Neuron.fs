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
  member this.Value = value;
  
  /// Adds an incoming connection from another Neuron
  member this.AddIncomingConnection c = connections <- c :: connections

  /// Adds all items in the collection together
  member this.Evaluate (numbers: decimal list) = 
    List.iter (fun (c:NeuronConnection) -> (value <- value + c.Calculate)) this.Inputs;

  