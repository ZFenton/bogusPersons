open System
open Bogus

[<CLIMutable>]
[<StructuredFormatDisplay("     {firstName} {lastName}'s email is {email}")>]
type Person = {firstName:string; lastName:string; email:string} // FirstName ... etc

let faker =
    Faker<Person>()
        .RuleFor( (fun p -> p.firstName), fun (f:Faker) -> f.Name.FirstName() )
        .RuleFor( (fun p -> p.lastName), fun (f:Faker) -> f.Name.LastName() )
        .RuleFor( (fun p -> p.email), fun (f:Faker) -> f.Internet.Email() )

    
[<EntryPoint>]
let main argv = 

    let persons = 100
    let personList = faker.Generate(persons) |> List.ofSeq
    let firstNameOnly person = person.firstName 
    let personsFirstNames = personList |> List.map firstNameOnly

    printfn "******************************************************************************************************************"
    printfn "*************************************** PRINTING ALL PERSONS' FIRST NAMES ****************************************\n"

    let formatNamesWithIndex = fun index firstName -> printfn "     Person %d) %s" (index + 1) firstName

    let printPersonNames = personsFirstNames |> List.iteri formatNamesWithIndex
    printPersonNames |> ignore
    printfn "\n******************************************************************************************************************"



    printfn "**************************************** PRINTING ALL PERSONS' With GMail ****************************************\n"

    let gmailDomain = "@gmail.com"
    let filterEmailOn emailDomain = 
                                    fun (person:Person) -> person.email.Contains(emailDomain)
    let personsWithGmailList = List.filter (filterEmailOn gmailDomain) personList

    let printRecordString person = printfn "%s" person
    
    let printPersonsWithGmail = 
                                printfn "   There are %d out of %d persons with gmail as their email:\n" personsWithGmailList.Length personList.Length
                                personsWithGmailList |> List.map string |> List.iter printRecordString
    let printIfNotEmpty = 
            match personsWithGmailList with
            | [] -> printfn "\n   -- No person has gmail as their email\n"
            | _ -> printPersonsWithGmail

    printIfNotEmpty |> ignore

    printfn "\n******************************************************************************************************************"



    printfn "************************************** PRINTING ALL PERSONS' NAMES REVERSED **************************************\n"

    let reverseString = Seq.rev >> Array.ofSeq >> System.String
    let reversedFirstName = fun person -> reverseString person.firstName

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