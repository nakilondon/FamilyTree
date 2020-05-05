import React, { Component } from 'react';
import BuildTimeLine from './buildTimeLine';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import GetImage from '../GetImage';
import { Typography, Grid } from '@material-ui/core';
import './PersonDetails.css'

export default class PersonTable extends Component {
    state ={}
    constructor(props) {
        super(props);
        this.state = {
            id: this.props.details.id,
            familyTable: null,
            timeLine: null,
            portrait: null
        }
    }

    onPersonChange = (e, id) => {
        this.props.selectedPerson(id);
    }
    
    componentDidUpdate() {
        if (this.props.details.id !== this.state.id) 
        {
            this.upDateState();
        }
    }

    componentDidMount() {
        this.upDateState();
    }

    upDateState() {
        let familyTable = this.buildFamilyTable(this.props.details.family);
        let timeLine = <BuildTimeLine events={this.props.details.events}/>
        let portrait = this.props.details.portrait === '' || this.props.details.portrait === null
        ? <div>No portrait</div> 
        : <img className="imageStyle" src={`familytree/img/${this.props.details.portrait}`}/>
       
        this.setState( {familyTable: familyTable, timeLine: timeLine, portrait: portrait, id: this.props.details.id})
    }

    buildFamilyTable(Family) {
        if (Family == null)
        {
           return <p>No Family</p>
        }
    
        return (
            <TableContainer component={Paper}>
                <Table className='table' aria-label="simple table">
                    <TableBody>
                        {Family
                        .map(member => (
                            <TableRow key={member.id} onClick={(event) => this.onPersonChange(event, member.id)}> 
                                <TableCell  component="th" scope="row">{member.relationship}</TableCell>
                                <TableCell  align="right">{member.name}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        );        
    }
    
    render() {
        return (
            <Grid container spacing={5} direction="column" justify="center" alignItems="center">
                <Grid item>
                    <Typography variant="h4">
                        {this.props.details.preferredName}
                    </Typography>
                </Grid>
                <Grid item >
                    {this.state.portrait}
                </Grid>
                <Grid item>
                    {this.state.familyTable}
                </Grid>
                <Grid item>
                    {this.state.timeLine}
                </Grid>
            </Grid>
        );
    }
}