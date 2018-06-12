open System
open Bogus

[<CLIMutable>]
[<StructuredFormatDisplay("     {FirstName} {LastName}'s email is {Email}")>]
type Person = {FirstName:string; LastName:string; Email:string}

let faker =
    Faker<Person>()
        .RuleFor( (fun p -> p.FirstName), fun (f:Faker) -> f.Name.FirstName() )
        .RuleFor( (fun p -> p.LastName), fun (f:Faker) -> f.Name.LastName() )
        .RuleFor( (fun p -> p.Email), fun (f:Faker) -> f.Internet.Email() )

    
[<EntryPoint>]
let main argv = 

    let persons = 10
    let personList = faker.Generate(persons) |> List.ofSeq
    let firstNameOnly person = person.FirstName 
    let personsFirstNames = personList |> List.map firstNameOnly

    printfn "******************************************************************************************************************"
    printfn "*************************************** PRINTING ALL PERSONS' FIRST NAMES ****************************************\n"

    let formatNamesWithIndex = fun index firstName -> printfn "     Person %d) %s" (index + 1) firstName

    let printPersonNames = personsFirstNames |> List.iteri formatNamesWithIndex
    printPersonNames
    printfn "\n******************************************************************************************************************"



    printfn "**************************************** PRINTING ALL PERSONS' With GMail ****************************************\n"

    let emailDomain = "@gmail.com"
    let filterEmailOn emailDomain = 
                                    fun (person:Person) -> person.Email.Contains(emailDomain)
    let personsWithGmailList = List.filter (filterEmailOn emailDomain) personList

    let printRecordString person = printfn "%s" person
    
    let printPersonsWithGmail = 
                                printfn "   There are %d out of %d persons with %s as their email:\n" personsWithGmailList.Length personList.Length emailDomain
                                personsWithGmailList |> List.map string |> List.iter printRecordString
    let printIfNotEmpty = 
            match personsWithGmailList with
            | [] -> printfn "\n   -- No person has %s as their email\n" emailDomain
            | _ -> printPersonsWithGmail

    printIfNotEmpty

    printfn "\n******************************************************************************************************************"



    printfn "************************************** PRINTING ALL PERSONS' NAMES REVERSED **************************************\n"

    let reverseString = Seq.rev >> Array.ofSeq >> System.String
    let reversedFirstName = fun person -> reverseString person.FirstName

    let printReversedFirstNames = personList |> List.map reversedFirstName |> List.iteri formatNamesWithIndex
    printReversedFirstNames

    printfn "\n******************************************************************************************************************"



    printfn "*********************************** PRINTING LETTERS FROM NAMES AS THEY APPEAR ***********************************\n"

    let distinctCharsOf stringList = stringList |> String.concat ""
                                                |> Seq.distinct
                                                |> Array.ofSeq
                                                |> System.String
    printfn "   All names with duplicate letters removed: %s" (distinctCharsOf personsFirstNames)

    printfn "\n******************************************************************************************************************\n"

    
    0