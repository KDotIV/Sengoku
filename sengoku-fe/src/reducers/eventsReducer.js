

const initState = {
    allEvents: [],
    searched: [],
}

const eventsReducer = (state = initState,action) => {
    switch(action.type)
    {
        case "FETCH_EVENTS":
            return {...state, 
                allEvents: action.payload.eventResult}
        default:
            return {...state}
    }
}

export default eventsReducer;