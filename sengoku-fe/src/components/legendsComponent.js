import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
//Redux
import { useDispatch } from 'react-redux';
import { getLegend } from '../actions/legendsAction'
import { Link } from 'react-router-dom';

const LegendComponent = ({ subject, summary, game, plotpoints, id }) => {
    //GetLegend
    const dispatch = useDispatch();
    const getLegendHandler = () => {
        dispatch(getLegend(id))
    };

    return(
        <CardStyle onClick={getLegendHandler}>
            <Link to={`/legends/${id}`}>
            <h3>{subject}</h3>
            <p>{summary}</p>
            <p>{game}</p>
            </Link>
        </CardStyle>
    );
}

const CardStyle = styled(motion.div)`
    min-height:30vh;
    box-shadow: 0px 5px 20px rgba(0,0,0,0.2);
    text-align: center;
    border-radius: 1rem;
    cursor: pointer;
    img{
        width: 100%;
        height: 40vh;
        object-fit: cover;
    }
`;

export default LegendComponent;