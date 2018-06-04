
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


    let persons = 1
    let personList = faker.Generate(persons) |> List.ofSeq


    printfn "\n*************** PRINTING ALL PERSONS' FIRST NAMES ****************\n"

    // QUESTION: how does compiler know this is parameter is "person -> string" when not explicitly specified?
    let firstNameOnly (person:Person) = person.firstName
    let formatNamesWithIndex = fun index firstName -> printfn "Person %d) %s" (index + 1) firstName


    let printPersonNames = personList |> List.map firstNameOnly |> List.iteri formatNamesWithIndex
    printPersonNames |> ignore
    printfn "\n******************************************************************\n"



    printfn "\n**************** PRINTING ALL PERSONS' With GMail ****************\n"

    let gmailDomain = "@gmail.com"
    let filterEmailOn emailDomain = 
        fun (person:Person) -> person.email.Contains(emailDomain)
    let personsWithGmailList = List.filter (filterEmailOn gmailDomain) personList

    // QUESTION: why does compiler refer to this parameter as " 'a -> string" when not specified? Is calling ToString() suspect here somehow?
    let personToString (person:Person) = person.ToString()
    let printRecordString person = printfn "%s" person
    
    let printPersonsWithGmail = personsWithGmailList |> List.map personToString |> List.iter printRecordString
    let printIfNotEmpty = 
            match List.isEmpty personsWithGmailList with
            | true -> printfn "\n -- No person has gmail as their email\n"
            | _ -> printPersonsWithGmail

    printIfNotEmpty |> ignore

    printfn "\n******************************************************************\n"



    printfn "\n**************** PRINTING ALL PERSONS' NAMES REVERSED ****************\n"

    let reverseString (x : string) = String (Array.rev (x.ToCharArray()))
    let reversedFirstName = fun person -> reverseString person.firstName

    let printReversedFirstNames = personList |> List.map reversedFirstName |> List.iteri formatNamesWithIndex
    printReversedFirstNames |> ignore
    printfn "\n**********************************************************************\n"



    printfn "\n**************** PRINTING LETTERS FROM NAMES AS THEY APPEAR ****************\n"

    let lettersInNamesAsAppeared : char array = Array.empty
    let firstNamesArray = personList |> List.map firstNameOnly

    // Go through all indexes in list/array
    // "add" the name from the first index to letter list/array (or build string?)
    // for every proceeding name, check it's letters against the letters in the name from the first index
    //        if the letter is not in the first name pulled, add the letter to the new string/array/list
    // return value will be a list/array/ new string containing all the letters that appeared in the names

    printfn "\n****************************************************************************\n"

    
    0