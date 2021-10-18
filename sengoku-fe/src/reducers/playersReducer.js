const initState = {
    allPlayers: [],
    searched: [],
}

const playersReducer = (state = initState,action) => {
    switch(action.type)
    {
        case "FETCH_PLAYERS":
            return {...state, 
                allPlayers: action.payload.playerResult}
        default:
            return {...state}
    }
}

export default playersReducer;