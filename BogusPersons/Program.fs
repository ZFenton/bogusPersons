
open System
open Bogus

[<CLIMutable>]
[<StructuredFormatDisplay("     {firstName} {lastName}'s email is {email}")>]
type Person = {firstName:string; lastName:string; email:string}

let faker =
    Faker<Person>()
        .RuleFor( (fun p -> p.firstName), fun (f:Faker) -> f.Name.FirstName() )
        .RuleFor( (fun p -> p.lastName), fun (f:Faker) -> f.Name.LastName() )
        .RuleFor( (fun p -> p.email), fun (f:Faker) -> f.Internet.Email() )

    
[<EntryPoint>]
let main argv = 

    let persons = 1000
    let personList = faker.Generate(persons) |> List.ofSeq
    let firstNameOnly (person:Person) = person.firstName     // QUESTION: how does compiler know this is parameter is "person -> string" when not explicitly specified?
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

    // QUESTION: why does compiler refer to this parameter as " 'a -> string" when not specified? Is calling ToString() suspect here somehow?
    let personToString (person:Person) = person.ToString()
    let printRecordString person = printfn "%s" person
    
    let printPersonsWithGmail = 
                                printfn "   There are %d out of %d persons with gmail as their email:\n" personsWithGmailList.Length personList.Length
                                personsWithGmailList |> List.map personToString |> List.iter printRecordString
    let printIfNotEmpty = 
            match List.isEmpty personsWithGmailList with
            | true -> printfn "\n   -- No person has gmail as their email\n"
            | _ -> printPersonsWithGmail

    printIfNotEmpty |> ignore

    printfn "\n******************************************************************************************************************"



    printfn "************************************** PRINTING ALL PERSONS' NAMES REVERSED **************************************\n"

    let reverseString (x : string) = String (Array.rev (x.ToCharArray()))
    let reversedFirstName = fun person -> reverseString person.firstName

    let printReversedFirstNames = personList |> List.map reversedFirstName |> List.iteri formatNamesWithIndex
    printReversedFirstNames |> ignore
    printfn "\n******************************************************************************************************************"



    printfn "*********************************** PRINTING LETTERS FROM NAMES AS THEY APPEAR ***********************************\n"

    let distinctCharsOf stringList = stringList |> String.concat ""
                                                |> Seq.distinct
                                                |> Array.ofSeq
                                                |> System.String
    printfn "   All names with duplicate letters removed: %s" (distinctCharsOf personsFirstNames)

    ////         OLD VERBOSE SOLUTION BY USING THE KEYWORD MUTABLE
    //let lower = ['a' .. 'z']
    //let upper = ['A' .. 'Z']
    //let mutable concatedNames = ["John"; "James"; "Jack"] |> List.fold (+) ""

    //let greaterThanOne (length : int) = length > 1
    //let stripChars (str : string) letter =
    //    let parts = str.Split([| letter |])
    //    match greaterThanOne (Array.length parts) with
    //    | true -> seq {
    //                    yield Array.head parts
    //                    yield string letter
    //                    yield! Array.tail parts
    //                  }
    //                  |> String.concat ""
    //    | _ -> str
    
    //let stripDuplicateLetter = fun letter -> concatedNames <- (stripChars concatedNames letter)
    //let stripAllButFirst list = list |> List.iter stripDuplicateLetter
    //stripAllButFirst lower
    //stripAllButFirst upper
    //printfn "All names with duplicate letters removed: %s" concatedNames

    printfn "\n******************************************************************************************************************\n"

    
    0