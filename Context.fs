module Context

open Gjallarhorn
open Gjallarhorn.Bindable
open Gjallarhorn.Validation
open Gjallarhorn.Validation.Validators


let create () =    
    // Create our binding source
    let source = Binding.createSource ()

    // Create two mutable values from our initial values
    let result = Mutable.create "hello world"

    let canExecute =  Mutable.create true

    // Display the results bound as "Result"
    Binding.toView source "Result" result

    let okCommand = source |> Binding.createCommandChecked "ChangeCommand" canExecute

    let okLogic(_) = 
        canExecute.Value <- false
        async{
        result.Value <- result.Value + " ! " 
        do!  Async.Sleep 3000 
        canExecute.Value <- true
        } |> Async.StartImmediate

    okCommand 
    |> Observable.subscribe(okLogic)
    |> source.AddDisposable

    source