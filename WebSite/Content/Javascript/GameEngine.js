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
    var sansWhites = RemoveWhitePositions(code, guess);
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