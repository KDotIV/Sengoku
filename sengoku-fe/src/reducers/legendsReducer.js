const initState = {
    allLegends: [],
    searched: [],
    getLegends: {},
}

const legendsReducer = (state = initState,action) => {
    switch(action.type)
    {
        case "FETCH_LEGENDS":
            return {...state, 
                allLegends: action.payload.legendResult}
        case "GET_LEGEND":
            return {...state,
                getLegend: action.payload.legendResult}
        default:
            return {...state}
    }
}

export default legendsReducer;
