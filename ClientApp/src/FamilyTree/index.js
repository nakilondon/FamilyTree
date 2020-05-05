import React, { Component } from 'react';
import FamilyTreeDiagram from './FamilyDiagram'
import Grid from '@material-ui/core/Grid';

export default class FamilyTree extends Component {
    constructor(props) {
        super(props);
        this.state = { 
            itemsData: [],
            title: null,
            loading: true
        };
    
    }

    componentDidMount() {
        this.populateItems();
    }

    render() {
        let contents = this.state.loading 
            ? <p><em>Loading...</em></p>
            : <FamilyTreeDiagram familyItems={this.state.itemsData} id={this.props.id} selectedPerson={this.props.selectedPerson}/>
        
        return (<Grid container align="center">{contents}</Grid>);
    }

    async populateItems() {
        const response = await fetch('familytree');
        const data = await response.json();
        this.setState({ itemsData: data, loading: false });
    }
}