
open System
open Bogus

[<CLIMutable>]
[<StructuredFormatDisplay("{firstName} {lastName}'s email is {email}")>]
type Person = {firstName:string; lastName:string; email:string}

let faker =
    Faker<Person>()
        .RuleFor( (fun p -> p.firstName), fun (f:Faker) -> f.Name.FirstName() )
        .RuleFor( (fun p -> p.lastName), fun (f:Faker) -> f.Name.LastName() )
        .RuleFor( (fun p -> p.email), fun (f:Faker) -> f.Internet.Email() )

[<EntryPoint>]
let main argv = 


    let persons = 10
    let personList = faker.Generate(persons) |> List.ofSeq


    printfn "\n*************** PRINTING ALL PERSONS' FIRST NAMES ****************\n"

    // QUESTION: how does compiler know this is parameter is "person -> string" when not explicitly specified?
    let firstNameOnly (person:Person) = person.firstName
    let formatNamesWithIndex = fun index firstName -> printfn "Person %d) %s" (index + 1) firstName


    let printPersonNames = personList |> List.map firstNameOnly |> List.iteri formatNamesWithIndex
    printPersonNames |> ignore
    printfn "\n******************************************************************\n"



    printfn "**************** PRINTING ALL PERSONS' With GMail ****************\n"

    let gmailDomain = "@gmail.com"
    let filterEmailOn emailDomain = 
        fun (person:Person) -> person.email.Contains(emailDomain)
    let personsWithGmailList = List.filter (filterEmailOn gmailDomain) personList

    // QUESTION: why does compiler refer to this parameter as " 'a -> string" when not specified? Is calling ToString() suspect here somehow?
    let personToString (person:Person) = person.ToString()
    let printRecordString person = printfn "%s" person
    
    let printPersonsWithGmail = personsWithGmailList |> List.map personToString |> List.iter printRecordString
    printPersonsWithGmail |> ignore
    printfn "\n******************************************************************\n"

    0