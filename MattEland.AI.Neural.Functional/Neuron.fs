namespace MattEland.AI.Neural.Functional

open System.Runtime.InteropServices

/// Represents a connection between two Neurons
type NeuronConnection(source: Neuron, ?initialWeight: decimal) = 

  let mutable weight: decimal  = defaultArg initialWeight 1M ;

  /// The neuron this connection comes from
  member this.Source: Neuron = source;

  /// The mathematical weight or importance of the connection
  member this.Weight
    with get() = weight
    and set(newWeight) = weight <- newWeight

  /// Calculates the value of the connection by evaluating the weight and the current value of the source neuron
  member this.Calculate = this.Weight * this.Source.Value;

/// Represents a node in a Neural Network
and Neuron ([<Optional>] ?initialValue: decimal) =

  let mutable value = defaultArg initialValue 0M;
  let mutable inputs: NeuronConnection seq = Seq.empty;

  /// Exposes the current calculated amount of the Neuron
  member this.Value
    with get () = value
    and set (newValue) = value <- newValue

  /// Incoming connections from other Neurons (if any)
  member this.Inputs: NeuronConnection seq = inputs;

  /// Adds an incoming connection from another Neuron
  member this.AddIncomingConnection c = inputs <- Seq.append this.Inputs [c];

  /// Adds all connections together, stores the result in Value, and returns the value
  member this.Evaluate(): decimal =
    value <- Seq.sumBy (fun (c:NeuronConnection) -> c.Calculate) this.Inputs;
    value;

/// A layer is just a series of Neurons in parallel that will link to every Neuron in the next layer (if any is present)
and NeuralNetLayer(numNeurons: int) =
  do if numNeurons <= 0 then invalidArg "numNeurons" "There must be at least one neuron in each layer";
    
  let neurons: Neuron seq = seq [ for i in 1 .. numNeurons -> new Neuron 0M]
  /// Layers should start with an empty collection of neurons
  member this.Neurons: Neuron seq = neurons;

  /// Sets the value of every neuron in the sequence to the corresponding ordered value provided
  member this.SetValues (values: decimal seq) = 
    let assignValue (n:Neuron) (v:decimal) = n.Value <- v;
    Seq.iter2 assignValue this.Neurons values
