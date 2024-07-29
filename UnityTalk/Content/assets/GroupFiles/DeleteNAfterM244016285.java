public class DeleteNAfterM {
        public class Node{
            int data;
            Node next;
            Node(int data){
                this.data = data;
                this.next = null;
            }
        }
        public static Node head;
        public static Node tail;
        public static int size;
        public void printLl(){
            if(head == null){
                System.out.println("The linked list is Empty");
                return;
            }
            Node temp = head;
            while(temp != null){
                System.out.print(temp.data+ " -> ");
                temp = temp.next;
            }
            System.out.println("null");
        }
        public void addFirst(int data){
            Node newNode = new Node(data); //New node
            size++; //Size inc
            if(head == null){ //If its node 1 then
                head = tail = newNode;
                return;
            }
            newNode.next = head; 
            head = newNode;
        }
        public void MnConverter(Node head,int m ,int n) {
            if(m == 0 && n == 0){
                return ;
            }
            int countM = 0;
            int countN = 0;
            Node temp = head;
            System.out.println("Outerloop"+temp.data);
            while (temp!=null) { 
                System.out.println("Inner loop"+temp.data + " "+ countM );
                countM++;
                if (countM == m && temp != null){
                    while(countN != n && temp != null){
                        System.out.println("Inner Inner loop"+temp.data);
                        Node del= temp.next;
                        if(del != null){
                            temp.next = del.next;
                        }else{
                            temp = null;
                        }

                        countN++;
                    }
                    countN = 0;
                    countM = 0;
                }
                if(temp == null){
                    return;
                }
                temp = temp.next;
            }
        }
        public static void main(String[] args) {
            DeleteNAfterM obj = new DeleteNAfterM();
            obj.addFirst(10);
            obj.addFirst(9);
            obj.addFirst(8);
            obj.addFirst(7);
            obj.addFirst(6);
            obj.addFirst(5);
            obj.addFirst(4);
            obj.addFirst(3);
            obj.addFirst(2);
            obj.addFirst(1);
            obj.printLl();
            obj.MnConverter(head, 2, 2);
            obj.printLl();
        }
}
