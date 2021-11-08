import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
//Redux
import { useDispatch } from 'react-redux';
import { getLegend } from '../actions/legendsAction'
import { Link } from 'react-router-dom';
import YoutubeEmbed from './youtubeComponent';

const PlotComponenet = ({ image, text, title, id, clipRef }) => {
    return (
        <PlotStyle>
        <h3>{title}</h3>
        <p>{text}</p>
        <img src={image} alt="images" />
        <YoutubeEmbed embedId={clipRef}/>
    </PlotStyle>
    );
}

const PlotStyle = styled(motion.div)`
    text-align: center;
    border-radius: 1rem;
    overflow: hidden;
    padding: 5rem;
    img{
        width: 100%;
        height: 40vh;
        object-fit: cover;
    }
`;

export default PlotComponenet;