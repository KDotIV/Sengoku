const initState = {
    allEvents: [],
    searched: [],
    getEvent: {},
    isLoading: true,
}

const eventsReducer = (state = initState,action) => {
    switch(action.type)
    {
        case "FETCH_EVENTS":
            return {...state, 
                allEvents: action.payload.eventResult}
        case "GET_EVENT":
            return {...state,
                getEvent: action.payload.eventResult}
        default:
            return {...state}
    }
}

export default eventsReducer;