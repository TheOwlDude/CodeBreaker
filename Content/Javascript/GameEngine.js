function RemoveBlackPositions(code, guess) {
    var newArrayCounter = 0;
    var newCode = [];
    var newGuess = [];
    var i;
    for (i = 0; i < code.length; ++i) {
        if (code[i] != guess[i]) {
            newCode[newArrayCounter] = code[i];
            newGuess[newArrayCounter] = guess[i];
            ++newArrayCounter;
        }
    }

    return { NewCode: newCode, NewGuess: newGuess };
}



function RemoveWhitePositions(code, guess) {
    
    var newCodeCounter = 0;
    var newCode = [];

    var cloneGuess = guess.slice(0);
    var i;
    for(i = 0; i < code.length; ++i) {
        var foundMatch = false;
        for (j = 0; j < cloneGuess.length; ++j) {
            if (code[i] == cloneGuess[j]) {
                foundMatch = true;
                cloneGuess.splice(j, 1);
                break;
            }
        }
        if (!foundMatch) newCode[newCodeCounter++] = code[i];
    }

    return { NewCode: newCode, NewGuess: cloneGuess };
}

function CalculateGuessResult(code, guess) {
    var sansBlacks = RemoveBlackPositions(code, guess);
    var sansWhites = RemoveWhitePositions(sansBlacks.NewCode, sansBlacks.NewGuess);
    return { BlackCount: code.length - sansBlacks.NewCode.length, WhiteCount: sansBlacks.NewCode.length - sansWhites.NewCode.length };
}

function ResultIsConsistentWithCode(code, guess, compareResult) {
    var guessResult = CalculateGuessResult(code, guess);
    return guessResult.BlackCount == compareResult.BlackCount && guessResult.WhiteCount == compareResult.WhiteCount;
}


function AreResultsConsistentWithCode(code, guessResults) {
    var i;
    for (i = 0; i < guessResults.length; ++i) {
        if (!ResultIsConsistentWithCode(code, guessResults[i].Guess, guessResults[i].Result)) return false;
    }
    return true;
}


//Call with two empty lists and after execution the first list wll be populated with all colorCount^codeLength codes based on codeLength and colorCount
function AddCompleteCodeToList(codeList, code, codeLength, colorCount) {
    if (code.length == codeLength) codeList.push(code);
    else {
        var i;
        for (i = 0; i < colorCount; ++i) {
            var newColor = [i];
            AddCompleteCodeToList(codeList, code.concat(newColor), codeLength, colorCount);
        }
    }
}


function StringifyCode(code) {
    var stringification = "[";
    var i;
    for (i = 0; i < code.length; ++i) {
        if (i != 0) stringification += ",";
        stringification += code[i];
    }
    return stringification += "]";
}


function CodesConsistentWithGuessResults(codeLength, colorCount, guessResults) {
    var allCodes = [];
    AddCompleteCodeToList(allCodes, [], codeLength, colorCount);    
    return allCodes.filter(
        function (element, index, array) {
            return AreResultsConsistentWithCode(element, guessResults);
        }
    );
    
}


function GetOutcomeCountMapForGuess(codeLength, colorCount, consistentCodes, guess) {
    var outcomeCountMap = [];
    consistentCodes.forEach(
        function (currentValue, index, array) {
            var guessResult = CalculateGuessResult(currentValue, guess);
            var guessResultIndex = GetIndexForGuessResult(guessResult.BlackCount, guessResult.WhiteCount);
            if (typeof outcomeCountMap[guessResultIndex] === 'undefined') {
                outcomeCountMap[guessResultIndex] = { BlackCount: guessResult.BlackCount, Weight: 1 };
            }
            else {
                outcomeCountMap[guessResultIndex].Weight++;
            }
        }
    );
    return outcomeCountMap;
}

function CalculateGuessQuality(codeLength, colorCount, consistentCodes, guess) {
    var outcomeCountMap = GetOutcomeCountMapForGuess(codeLength, colorCount, consistentCodes, guess);
    var quality = outcomeCountMap.reduce(
        function (previousValue, currentValue, currentIndex, array) {
            return previousValue + (currentValue.BlackCount == codeLength ? 0 : (currentValue.Weight * currentValue.Weight) / consistentCodes.length);
        },
        0
    );
    return { Guess: guess, Quality: quality };
}


function GetGuessesSortedByQuality(codeLength, colorCount, guessResults) {
    var consistentCodes = CodesConsistentWithGuessResults(codeLength, colorCount, guessResults);
    var allCodes = [];
    AddCompleteCodeToList(allCodes, [], codeLength, colorCount);
    var codesWithQuality = allCodes.map(
        function (currentValue, index, array) {
            return CalculateGuessQuality(codeLength, colorCount, consistentCodes, currentValue);
        }
    );
    return codesWithQuality.sort(
        function (a, b) {
            if (a.Quality < b.Quality) return -1;
            if (a.Quality > b.Quality) return 1;
            return 0;
        }
    );
}

//Creates a unique index for a guess result. 
function GetIndexForGuessResult(blackCount, whiteCount) {
    return (blackCount << 5) | whiteCount;
}

function GetGuessResultFromIndex(index) {
    return { BlackCount: index >> 5, WhiteCount: index % 16 };
}

