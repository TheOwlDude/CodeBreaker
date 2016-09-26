module Game.Engine

type CodeAndGuess = { code : int list; guess : int list }

type Outcome = { blackCount: int; whiteCount: int}

type GuessOutcome = { guess : int list; result: Outcome }

type ItemCount<'a> = { item : 'a; count: int }


let rec removeBlackPositions (codeAndGuess:CodeAndGuess) = 
  if codeAndGuess.code.IsEmpty then codeAndGuess
  else     
    if codeAndGuess.code.Head = codeAndGuess.guess.Head then removeBlackPositions {code=codeAndGuess.code.Tail; guess=codeAndGuess.guess.Tail}
    else let removedBlacksFromTail = removeBlackPositions {code=codeAndGuess.code.Tail; guess=codeAndGuess.guess.Tail} in {code=codeAndGuess.code.Head::removedBlacksFromTail.code;guess=codeAndGuess.guess.Head::removedBlacksFromTail.guess}

//Given a code list and a value returns the code with the first item matching the value removed if it appears, else the original code
let rec removeWhiteMatch (guessValue:int) (code: int list) =
    if code.IsEmpty then code
    else 
      if guessValue = code.Head then code.Tail
      else code.Head :: removeWhiteMatch guessValue code.Tail

//Given a code list and guess list, returns shortened code and guess lists with the white positions removed.
let rec removeWhitePositions (codeAndGuess:CodeAndGuess) = 
  if codeAndGuess.guess.IsEmpty then codeAndGuess
  else     
    let remainingCode = removeWhiteMatch codeAndGuess.guess.Head codeAndGuess.code in 
          if remainingCode.Length <> codeAndGuess.code.Length then removeWhitePositions {code=remainingCode; guess=codeAndGuess.guess.Tail} 
          else 
            let removedWhitePositions = removeWhitePositions {code=remainingCode; guess=codeAndGuess.guess.Tail} in {code=removedWhitePositions.code; guess = codeAndGuess.guess.Head::removedWhitePositions.guess}

let calculateGuessResult (guess : int list) (code : int list) =
    let blacksRemoved = removeBlackPositions {code=code;guess=guess} in
        let whitesRemoved = removeWhitePositions blacksRemoved in
            {blackCount = code.Length - blacksRemoved.code.Length; whiteCount = blacksRemoved.code.Length - whitesRemoved.code.Length}

//Determines if the supplied guessOutcome is consistent with the supplied code
let outcomeIsConsistentWithCode (code: int list) (guessOutcome: GuessOutcome) =
    let outcome = calculateGuessResult guessOutcome.guess code in 
           outcome.blackCount = guessOutcome.result.blackCount &&
           outcome.whiteCount = guessOutcome.result.whiteCount
             
//Determines if a list of guess outcomes are all consistent with the supplied code
let areGuessOutcomesConsistentWithCode (guessOutcomes: GuessOutcome list) (code: int list) = 
    List.forall (outcomeIsConsistentWithCode code) guessOutcomes
       

//Recursively adds codes not to exceed the supplied length or max color to the supplied list
let rec generateAndAddCodes (codeLength : int) (colorCount : int) (codeSoFar : int list) (nextColor : int) (codeList : (int list) list) =
    if (codeSoFar.Length = codeLength) then codeSoFar :: codeList
    else 
      if (nextColor >= colorCount) then codeList
      else List.append (generateAndAddCodes codeLength colorCount codeSoFar (nextColor + 1) codeList) (generateAndAddCodes codeLength colorCount (nextColor :: codeSoFar) 0 codeList)

//Generates all possible codes for the supplied length and coloc count
let generateCodes (codeLength : int) (colorCount : int) = generateAndAddCodes codeLength colorCount [] 0 []

//Returns the subset of all possible codes that are consistent with the supplied list of GuessOutcomes
let codesConsistentWithOutcomes (codeLength : int) (colorCount : int) (guessOutcomes : GuessOutcome list) = 
    List.filter (areGuessOutcomesConsistentWithCode guessOutcomes) (generateCodes codeLength colorCount)

//The supplied Map<Outome,int> contains the count of like outcomes seen to date. Creates a map with the existing count incremented if the supplied Outcome is in it
//else adds the item with count of 1
let incrementOutcomeInMap (codeLength : int) (map : Map<Outcome,int>) (outcome : Outcome) = 
    if Map.containsKey outcome map then 
      let priorCount = Map.find outcome map in Map.add outcome (priorCount + 1) (Map.remove outcome map)
    else 
        if outcome.blackCount = codeLength then Map.add outcome 0 map
        else Map.add outcome 1 map

//Creates a weighted map of possible outcomes for a new guess based on the historical GuessOutcomes to date. It does this by calculating for each code consistent with the set of GuessOutcomes to date
//the outcome for the new guess.
let getOutcomeCountMapForGuess (codeLength : int) (colorCount : int) (consistentCodes : (int list) list) (outcomesToDate : GuessOutcome list) (nextGuess : int list) =
   let possibleOutcomes = List.map (calculateGuessResult nextGuess) consistentCodes in
      List.fold (incrementOutcomeInMap codeLength) Map.empty possibleOutcomes
      

let AccumulateMapWeight (codeLength : int) (weightToDate : int) (outcome : Outcome) (currentOutcomeWeight : int) = weightToDate + if outcome.blackCount = codeLength then 1 else currentOutcomeWeight


//let GetWeightedCodeLength (weightToDate : int) (outcome : Outcome) (weightAndCodeLength) = 
let GetWeightedCodeLength (weightToDate : int) (outcome : Outcome) (weight : int) =   
    weight * weight + weightToDate


//Calculates the expected possible codes remaining after a new guess. For each outcome in the weighted map of possible outcomes the GuessOutcome of the new guess and possible outcome is added 
//to the game's GuessOutcome list and the list of possible codes recalculated. The average of the counts of these recalculated possible code lists weighted by the map weights is the answer.
let calculateExpectedRemainingPossibleCodes (codeLength : int) (colorCount : int) (consistentCodes : (int list) list) (outcomesToDate : GuessOutcome list) (nextGuess : int list) =
    let weightedPossibleOutcomes = getOutcomeCountMapForGuess codeLength colorCount consistentCodes outcomesToDate nextGuess in
      let totalWeight = Map.fold (AccumulateMapWeight codeLength) 0 weightedPossibleOutcomes in
        let weightedSum = Map.fold GetWeightedCodeLength 0 weightedPossibleOutcomes in
          (nextGuess, (decimal weightedSum) / if totalWeight = 0 then (decimal 1) else (decimal totalWeight))


let selectValueFromTuple codeValueTuple =
  let (code, value) = codeValueTuple
    in value

let getGuessesSortedByQuality (codeLength : int) (colorCount : int) (outcomesToDate : GuessOutcome list) =
  let allGuesses = generateCodes codeLength colorCount in
    let consistentCodes = codesConsistentWithOutcomes codeLength colorCount outcomesToDate in
      let possibleCodes = codesConsistentWithOutcomes codeLength colorCount outcomesToDate in
        List.sortBy selectValueFromTuple (List.map (calculateExpectedRemainingPossibleCodes codeLength colorCount consistentCodes outcomesToDate) allGuesses)
    

    

