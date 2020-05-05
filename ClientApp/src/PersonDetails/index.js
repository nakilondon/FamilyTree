import React, { Component } from 'react';
import PersonTable from './buildTable';
import Grid from '@material-ui/core/Grid';
import './PersonDetails.css'

export default class PersonDetails extends Component {
  static displayName = PersonDetails.name;
  
  constructor(props) {
    super(props);
    this.state = { Details: [], loading: true, id: this.props.id };
  }

  componentDidMount() {
    this.populatePersonData(this.props.id);
  }

  componentDidUpdate() {
    if (this.state.id !== this.props.id)
    {
      this.setState({loadding: true, id: this.props.id});
      this.populatePersonData(this.props.id);      
    }
  }

  render() {
     let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : <PersonTable details={this.state.Details} selectedPerson={this.props.selectedPerson}/>

    return (
      <Grid container space={5} direction="row" justify="center" alignItems="center">
        <Grid item>
          {contents}
        </Grid>
      </Grid>
    );
  }

  async populatePersonData(id) {
    const response = await fetch(`familytree/${id}`);
    const data = await response.json();
    this.setState({ Details: data, loading: false });
  }
}