import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";

const Events = ({ eventName, eventLocation, eventGame, eventAddress }) => {
    return(
        <div>
            <h3>{eventName}</h3>
            <p>{eventLocation}</p>
            <p>{eventGame}</p>
        </div>
    );
}

export default Events;
