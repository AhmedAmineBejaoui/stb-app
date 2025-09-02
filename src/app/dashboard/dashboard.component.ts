import { Component } from '@angular/core';

interface Demande {
  id: string;
  type: string;
  objet: string;
  date: string;
  statut: 'En attente' | 'Acceptée' | 'Rejetée';
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  filterStatus: string = 'Tout';
  demandes: Demande[] = [
    { id: 'STB-001', type: 'Support technique', objet: 'Problème de connexion', date: '2025-07-01', statut: 'En attente' },
    { id: 'STB-002', type: 'Demande de congé', objet: 'Congé annuel', date: '2025-07-02', statut: 'Acceptée' },
    { id: 'STB-003', type: 'Requête', objet: 'Mise à jour profil', date: '2025-07-03', statut: 'Rejetée' },
  ];


  selected: Demande | null = null;


  get filteredDemandes(): Demande[] {
    if (this.filterStatus === 'Tout') {
      return this.demandes;
    }
    return this.demandes.filter(d => d.statut === this.filterStatus);
  }

  showDetails(d: Demande) {

    this.selected = d;
  }

  closeDetails() {
    this.selected = null;

  }
}
