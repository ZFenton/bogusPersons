
open System
open Bogus

[<CLIMutable>]
type Person = {FirstName:string; LastName:string; Email:string}

let faker =
    Faker<Person>()
        .RuleFor( (fun p -> p.FirstName), fun (f:Faker) -> f.Name.FirstName() )
        .RuleFor( (fun p -> p.LastName), fun (f:Faker) -> f.Name.LastName() )
        .RuleFor( (fun p -> p.Email), fun (f:Faker) -> f.Internet.Email() )

[<EntryPoint>]
let main argv = 

    //let MODEL = personList |> List.iteri (fun i x -> printfn "Person %d) %s" (i + 1) x.FirstName) //FUNCTION TO USE AS MODEL FOR NEW FUNCTIONS
   (*********************************************************************
    //  DEPRECATED - BUT KEEPING HERE AS EXAMPLE OF SIMPLIFICATION
    let generatePerson() = faker.Generate(1)
    let whereNumberOfPersonsIs theGivenNumber = [1 .. theGivenNumber]
    let singlePerson =
        fun _ -> generatePerson() 
                   |> List.ofSeq 
                   |> List.exactlyOne

    let mapSinglePerson = List.map singlePerson
    let personList = whereNumberOfPersonsIs 10  |> mapSinglePerson
    **********************************************************************)
    
    let persons = 10
    let robustPersonList = faker.Generate(persons) |> List.ofSeq

    printfn "*************** PRINTING ALL PERSONS' FIRST NAMES ***************"
    let printNamesWithIndex index person = printfn "Person %d) %s" (index + 1) person.FirstName
    let printNumberedNames =
        fun index person -> printNamesWithIndex index person
    let asNumberedList = List.iteri printNumberedNames      // TODO: convert to map then print in last step !-- ALL PRINT FUNCTIONS SHOULD BE RE-USABLE --! 
    let printNamesFrom givenList = givenList |> asNumberedList
    printNamesFrom robustPersonList
    printfn "*****************************************************************\n"

    let gmailDomain = "@gmail.com"
    let filterOnEmail domain = 
        fun (person:Person) -> person.Email.Contains(domain)
    let personsWithGmailList = List.filter (filterOnEmail gmailDomain) robustPersonList

    printfn "**************** PRINTING ALL PERSONS' With GMail ****************"
    printfn "%A" personsWithGmailList

    let retval = 0
    retval