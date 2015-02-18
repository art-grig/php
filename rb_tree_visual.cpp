#include <iostream>
#include <string.h>
#include <fstream>
using namespace std;


template <class K,class E>
struct RB_Node {
	K key;
	E element;
	int color;
	RB_Node* left;
	RB_Node* right;
	RB_Node* parent;
};

template <class K,class E>
class RB_Tree {
	RB_Node<K,E>* NIL;
	RB_Node<K,E>* root;
	int size;
public:
	RB_Tree () {
		NIL = new RB_Node<K,E>;
		size = 0;
		NIL->key = 0;
		NIL->color = 1; 
	}
	void insert (K key,E element) {
		if (size == 0) {
			root = new RB_Node<K,E>;
			root->color = 1;
			root->key = key;
			root->element = element;
			root->right = NIL;
			root->left = NIL;
			root->parent = NIL;
			++size;
		}
		else {
			RB_Node<K,E>* buffer = new RB_Node<K,E>;
			RB_Node<K,E>* buffer_1 = new RB_Node<K,E>;
			RB_Node<K,E>* new_element = new RB_Node<K,E>;
			new_element->right = NIL;
			new_element->left = NIL;
			new_element->element = element;
			new_element->key = key;
			new_element->color = 0;
			buffer = root;
			while ( buffer!=NIL ) { 
				if (key<=buffer->key) { 
					buffer_1 = buffer;
					buffer = buffer->left;
				}
				if (buffer!=NIL && key>buffer->key) { //Может стать buffer == NIL - true !!!
					buffer_1 = buffer; 
					buffer = buffer->right;
				}
			}
			if (buffer==NIL) buffer = buffer_1;
			new_element->parent = buffer;
			if (key>buffer->key) buffer->right = new_element;
			else buffer->left = new_element; 
			++size;
			rb_insert_fixup(new_element);
		}
	}
	RB_Node<K,E>* max () {}
	RB_Node<K,E>* min () {}
	void del () {}
	RB_Node<K,E>* search () {}
	RB_Node<K,E>* Predecessor () {}
	RB_Node<K,E>* Successor () {}
	void left_rotate (RB_Node<K,E>* future_left) {
		RB_Node<K,E>* future_parent = new RB_Node<K,E>;
		future_parent = future_left->right;
		future_left->right = future_parent->left;
		if (future_parent->left!=NIL) future_parent->left->parent = future_left;
		future_parent->parent = future_left->parent;
		if (future_parent->parent == NIL ) root = future_parent; 
		else {
			if (future_left == future_left->parent->left) future_left->parent->left = future_parent;
			else future_left->parent->right = future_parent; //Ошибка потрачена 
		}
		future_parent->left = future_left;
		future_left->parent = future_parent;
	}

	void right_rotate (RB_Node<K,E>* future_right) {
		RB_Node<K,E>* future_parent = new RB_Node<K,E>;
		future_parent = future_right->left;
		future_right->left = future_parent->right;
		if (future_right->left!=NIL) future_parent->right->parent = future_right;
		future_parent->parent = future_right->parent;
		if (future_parent->parent==NIL) root = future_parent;
		else {
			if (future_right == future_right->parent->right) future_right->parent->right = future_parent;
			else future_right->parent->left = future_parent;			
		}
		future_parent->right = future_right;
		future_right->parent = future_parent;		
	}
	void rb_insert_fixup (RB_Node<K,E>* z) {
		RB_Node<K,E>* buffer = new RB_Node<K,E>;
		while (z->parent->color == 0) {
			if (z->parent == z->parent->parent->left) {
				buffer = z->parent->parent->right;
				if (buffer->color == 0) {
					z->parent->color = 1;
					buffer->color = 1;
					z->parent->parent->color = 0;
					z = z->parent->parent;
				}
				else {
					if (z == z->parent->right) {
						z = z->parent;
						left_rotate(z);
					}
					z->parent->color = 1;
					z->parent->parent->color = 0;
					right_rotate(z->parent);
				}
			}
			else {
				buffer = z->parent->parent->left;
				if (buffer->color == 0) {
					z->parent->color = 1;
					buffer->color = 1;
					z->parent->parent->color = 0;
					z = z->parent->parent;
				}
				else {
					if (z == z->parent->left) {
						z = z->parent;
						right_rotate(z);
					}
					else {
						z->parent->color = 1;
						z->parent->parent->color = 0;
						left_rotate(z->parent);
					}
				}
			}
		}
		root->color = 1;
	}
	void predstavlenie_dereva (RB_Node<K,E>* golova) {
		if (golova!=NIL) {
			predstavlenie_dereva(golova->left);
			cout << "Key: " << golova->key << " " << "Element: " << golova->element << " ";
			if (golova->left!=NIL)
				cout << "Left: " << golova->left->key << " ";
			else 	cout << "Left: " << 0 << " ";
			if (golova->right!=NIL) 
				cout << "Right: " << golova->right->key << " ";
			else 	   cout << "Right: " << 0 << " ";
			if (golova->parent!=NIL)
				cout << "Parent: " << golova->parent->key << endl;
			else 	cout << "Parent: " << 0 << endl;
			predstavlenie_dereva(golova->right);
		}
	}
	
	void predstavlenie_dereva() {
		predstavlenie_dereva(root);
	}
	RB_Node<K,E>* rb_search (RB_Node<K,E>* golova, K key) {
		if (golova==NULL || key == golova->key) return golova;
		if (key<=golova->key) return rb_search(golova->left,key);
		else return rb_search(golova->right,key);
	}
	
	RB_Node<K,E>* rb_search (K key) {
		return rb_search(root,key);
	}
	void consol_show (RB_Node<K,E>* golova, int a, int ind_left, int ind_right) {
		if (golova!=NIL) {
			if (golova == root) {
				space(a);
				cout << root->key;
				space(a);
			}
			++ind_left;
			space(a/2);
			cout << golova->left->key;
			space(a/2);
			cout << golova->right->key;
			space(a/2);
			if (ind_left %2 == 1) {
				if (ind_right%2 == 1) {
					golova = golova->parent->left->left;
				}  
				consol_show(golova->left, a/2,ind_left,ind_right);
			}
			else {
				++ind_right;
				consol_show(golova->parent->right, a/2, ind_left, ind_right);
			}
		}
	}	
	void consol_show () {
		consol_show(root,68,0,0);
	}
	void html_show (ostream& out,RB_Node<K,E>* t) {
		 const string color = (t->color == 1) ? "black" : "red" ;
  out << "<table cols=3 rows=2 style='vertical-align:top;font-family:arial;font-size:14pt'>"
      << "<tr><td>"
      << "<table cols=3 rows=2 width=100% height=100%>"
      << "<tr><td>&nbsp;</td>"
      << "<td style='border-bottom:solid'>&nbsp;</td></tr>"
      << "<tr><td style='border-right:solid'>&nbsp;</td><td></td></tr></table>"
      << "</td><td style='background-color:"
      << color << ";color:yellow; text-align:center'>" 
      << "Key: " << t->key << "\n" << "Element: " << t->element << "</td><td>"
      << "<table cols=2 rows=2 width=100% height=100%>"
      << "<tr><td style='border-bottom:solid'>&nbsp;</td>"
      << "<td>&nbsp;</td></tr>"
      << "<tr><td style='border-right:solid'>&nbsp;</td><td></td></tr></table>"
      << "</td></tr><tr>"
      << "<td style='vertical-align:top'>" ;
  if ( t->left!=NIL ) html_show(out,t->left) ;
  out << "</td><td></td><td style='vertical-align:top'>" ;
  if ( t->right!=NIL ) html_show(out,t->right) ;
  out << "</td></tr></table>" ;
	}
	void html_show (ostream& out) {
		html_show(out,root);
	}
	void tree_filling (istream& in) {
		static K key;
		static E element;
		static K last_key;
		static E last_element;
		while (in) {
			in >> key;
			in >> element;
			if (key!=last_key or element!=last_element)
			insert(key,element);
			last_key = key;
			last_element = element;
		}
	}
	
	template <class X, class Y> friend ostream& operator << (ostream&, RB_Tree<X,Y>) ;
	template <class X, class Y> friend istream& operator >> (istream&, RB_Tree<X,Y>&) ;
	void file_filling() {
	ifstream in;
	ofstream out;
	in.open("rb_filling.txt");
	out.open("rb_tree.html");
		static K key;
		static E element;
		static K last_key;
		static E last_element;
		while (in) {
			in >> key;
			in >> element;
			if (key!=last_key or element!=last_element)
			insert(key,element);
			last_key = key;
			last_element = element;
		}
	}
};
	
	template<class K, class E> ostream& operator << (ostream& out, RB_Tree<K, E> t) {
	t.html_show(out);
	return out;
}
	template <class K, class E> istream& operator >> (istream& in,RB_Tree<K,E>& t) {
	t.tree_filling(in);
	return in;
}






int main () {
	RB_Tree<int, string> vovan;
	/*vovan.insert(15,"art");
	vovan.insert(5,"vova");
	vovan.insert(18,"serg");
	vovan.insert(17,"sofa");
	vovan.insert(20,"nad");
	vovan.insert(7,"lyub");
	vovan.insert(6,"xxx");
	vovan.insert(13,"trans");
	vovan.insert(9,"pred");
	vovan.insert(3,"uno");
	vovan.insert(2,"dos");
	vovan.insert(4,"tres");*/
	vovan.file_filling();
	cout << vovan;
	//vovan.left_rotate(vovan.rb_search(20));
	//vovan.left_rotate(vovan.rb_search(15));
  	vovan.left_rotate(vovan.rb_search(15));
  	cout << vovan;
	//vovan.predstavlenie_dereva();
/*	vovan.right_rotate(vovan.rb_search(15));
  vovan.html_show();
	vovan.right_rotate(vovan.rb_search(5));
	//cout << "After rigth rotate:" << endl;
	//vovan.predstavlenie_dereva();
	//vovan.consol_show();
  vovan.html_show() ;
	vovan.right_rotate(vovan.rb_search(3));
  vovan.html_show() ;
	vovan.right_rotate(vovan.rb_search(7));
  vovan.html_show() ;
*/
	return 0;
}
